using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;
using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;
using Foundatio.CommandQuery.MongoDB.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.MongoDB.Tests;

public class RoleTests(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<RoleCreateModel>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence());

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<RoleCreateModel, RoleReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<RoleReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (RoleReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBeNullOrEmpty();
        created.Name.Should().Be(createModel.Name);
        created.Description.Should().Be(createModel.Description);

        // get entity
        var getCommand = new GetEntity<string, RoleReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<RoleReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (RoleReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.Name.Should().Be(createModel.Name);
        readModel.Description.Should().Be(createModel.Description);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(RoleReadModel.Name) }]
        };

        var queryCommand = new QueryEntities<RoleReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<RoleReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<RoleReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        RoleUpdateModel updateRequest = new()
        {
            Name = createModel.Name + " Updated",
            Description = createModel.Description + " Updated",
        };
        var updateCommand = new UpdateEntity<string, RoleUpdateModel, RoleReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<RoleReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (RoleReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.Name.Should().Be(updateRequest.Name);
        updatedModel.Description.Should().Be(updateRequest.Description);

        // delete entity
        var deleteCommand = new DeleteEntity<string, RoleReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<RoleReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }
}
