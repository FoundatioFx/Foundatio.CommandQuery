using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;
using Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;
using Foundatio.CommandQuery.EntityFramework.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

public class StatusTests : DatabaseTestBase
{
    public StatusTests(ITestOutputHelper output, DatabaseFixture databaseFixture)
        : base(output, databaseFixture)
    {
    }

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<StatusCreateModel>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.DisplayOrder, f => f.Random.Int(1, 100))
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<StatusCreateModel, StatusReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<StatusReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (StatusReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBe(0);
        created.Name.Should().Be(createModel.Name);
        created.Description.Should().Be(createModel.Description);
        created.DisplayOrder.Should().Be(createModel.DisplayOrder);

        // get entity
        var getCommand = new GetEntity<int, StatusReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<StatusReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (StatusReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.Name.Should().Be(createModel.Name);
        readModel.Description.Should().Be(createModel.Description);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(StatusReadModel.Name) }]
        };

        var queryCommand = new QueryEntities<StatusReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<StatusReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<StatusReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        StatusUpdateModel updateRequest = new()
        {
            Name = createModel.Name + " Updated",
            Description = createModel.Description + " Updated",
            DisplayOrder = createModel.DisplayOrder + 1,
            IsActive = !createModel.IsActive
        };
        var updateCommand = new UpdateEntity<int, StatusUpdateModel, StatusReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<StatusReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (StatusReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.Name.Should().Be(updateRequest.Name);
        updatedModel.Description.Should().Be(updateRequest.Description);

        // delete entity
        var deleteCommand = new DeleteEntity<int, StatusReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<StatusReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }
}
