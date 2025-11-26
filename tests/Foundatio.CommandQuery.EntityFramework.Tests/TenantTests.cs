using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;
using Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;
using Foundatio.CommandQuery.EntityFramework.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

public class TenantTests(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<TenantCreateModel>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<TenantCreateModel, TenantReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<TenantReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (TenantReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBe(0);
        created.Name.Should().Be(createModel.Name);
        created.Description.Should().Be(createModel.Description);

        // get entity
        var getCommand = new GetEntity<int, TenantReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<TenantReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (TenantReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.Name.Should().Be(createModel.Name);
        readModel.Description.Should().Be(createModel.Description);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(TenantReadModel.Name) }]
        };

        var queryCommand = new QueryEntities<TenantReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<TenantReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<TenantReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        TenantUpdateModel updateRequest = new()
        {
            Name = createModel.Name + " Updated",
            Description = createModel.Description + " Updated",
            IsActive = !createModel.IsActive
        };
        var updateCommand = new UpdateEntity<int, TenantUpdateModel, TenantReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<TenantReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (TenantReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.Name.Should().Be(updateRequest.Name);
        updatedModel.Description.Should().Be(updateRequest.Description);

        // delete entity
        var deleteCommand = new DeleteEntity<int, TenantReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<TenantReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }
}
