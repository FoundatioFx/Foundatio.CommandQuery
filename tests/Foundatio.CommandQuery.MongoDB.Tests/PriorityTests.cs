using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.MongoDB.Tests.Constants;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;
using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;
using Foundatio.CommandQuery.MongoDB.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.MongoDB.Tests;

public class PriorityTests : DatabaseTestBase
{
    public PriorityTests(ITestOutputHelper output, DatabaseFixture databaseFixture)
        : base(output, databaseFixture)
    {
    }

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<PriorityCreateModel>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.DisplayOrder, f => f.Random.Int(1, 100))
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<PriorityCreateModel, PriorityReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (PriorityReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBeNullOrEmpty();
        created.Name.Should().Be(createModel.Name);
        created.Description.Should().Be(createModel.Description);
        created.DisplayOrder.Should().Be(createModel.DisplayOrder);

        // get entity
        var getCommand = new GetEntity<string, PriorityReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (PriorityReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.Name.Should().Be(createModel.Name);
        readModel.Description.Should().Be(createModel.Description);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(PriorityReadModel.Name) }]
        };

        var queryCommand = new QueryEntities<PriorityReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<PriorityReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<PriorityReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        PriorityUpdateModel updateRequest = new()
        {
            Name = createModel.Name + " Updated",
            Description = createModel.Description + " Updated",
            DisplayOrder = createModel.DisplayOrder + 1,
            IsActive = !createModel.IsActive
        };
        var updateCommand = new UpdateEntity<string, PriorityUpdateModel, PriorityReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (PriorityReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.Name.Should().Be(updateRequest.Name);
        updatedModel.Description.Should().Be(updateRequest.Description);

        // delete entity
        var deleteCommand = new DeleteEntity<string, PriorityReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetEntity()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var getCommand = new GetEntity<string, PriorityReadModel>(MockPrincipal.Default, PriorityConstants.Normal.Id);
        var getResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (PriorityReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(PriorityConstants.Normal.Id);
        readModel.Name.Should().Be(PriorityConstants.Normal.Name);
        readModel.Description.Should().Be(PriorityConstants.Normal.Description);
    }

    [Fact]
    public async Task GetEntities()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        string[] ids =
        [
            PriorityConstants.Low.Id,
            PriorityConstants.Normal.Id,
            PriorityConstants.High.Id,
        ];
        var command = new GetEntities<string, PriorityReadModel>(MockPrincipal.Default, ids);
        var result = await mediator.InvokeAsync<Result<IReadOnlyList<PriorityReadModel>>>(command);
        result.Should().NotBeNull();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();

        var readModels = result.Value;
        readModels.Should().NotBeNull();
        readModels.Should().HaveCount(3);

        var normalModel = readModels.FirstOrDefault(m => m.Id == PriorityConstants.Normal.Id);
        normalModel.Should().NotBeNull();
        normalModel.Name.Should().Be(PriorityConstants.Normal.Name);
        normalModel.Description.Should().Be(PriorityConstants.Normal.Description);
    }

    [Fact]
    public async Task QueryEntities()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var filter = QueryBuilder.Search<PriorityReadModel>("Normal");
        var sort = QueryBuilder.Sort<PriorityReadModel>();

        var definition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Filter = filter,
            Sorts = [sort]
        };

        var command = new QueryEntities<PriorityReadModel>(MockPrincipal.Default, definition);
        var result = await mediator.InvokeAsync<Result<QueryResult<PriorityReadModel>>>(command);
        result.Should().NotBeNull();

        var queryModel = (QueryResult<PriorityReadModel>)result;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);
    }
}
