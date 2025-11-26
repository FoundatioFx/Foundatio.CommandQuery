using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;
using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;
using Foundatio.CommandQuery.MongoDB.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.MongoDB.Tests;

public class UserTests(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<UserCreateModel>()
            .RuleFor(p => p.EmailAddress, f => f.Internet.Email())
            .RuleFor(p => p.DisplayName, f => f.Name.FullName())
            .RuleFor(p => p.PasswordHash, f => f.Random.AlphaNumeric(20))
            .RuleFor(p => p.ResetHash, f => f.Random.AlphaNumeric(20))
            .RuleFor(p => p.InviteHash, f => f.Random.AlphaNumeric(20));

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<UserCreateModel, UserReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<UserReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (UserReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBeNullOrEmpty();
        created.EmailAddress.Should().Be(createModel.EmailAddress);
        created.DisplayName.Should().Be(createModel.DisplayName);

        // get entity
        var getCommand = new GetEntity<string, UserReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<UserReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (UserReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.EmailAddress.Should().Be(createModel.EmailAddress);
        readModel.DisplayName.Should().Be(createModel.DisplayName);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(UserReadModel.EmailAddress) }]
        };

        var queryCommand = new QueryEntities<UserReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<UserReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<UserReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        UserUpdateModel updateRequest = new()
        {
            EmailAddress = "Update." + createModel.EmailAddress,
            DisplayName = createModel.DisplayName + " Updated",
        };
        var updateCommand = new UpdateEntity<string, UserUpdateModel, UserReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<UserReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (UserReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.EmailAddress.Should().Be(updateRequest.EmailAddress);
        updatedModel.DisplayName.Should().Be(updateRequest.DisplayName);

        // delete entity
        var deleteCommand = new DeleteEntity<string, UserReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<UserReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }
}
