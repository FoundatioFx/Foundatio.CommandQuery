using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.EntityFramework.Tests.Constants;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;
using Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;
using Foundatio.CommandQuery.EntityFramework.Tests.Mocks;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

public class TaskTests(DatabaseFixture databaseFixture) : DatabaseTestBase(databaseFixture)
{

    [Fact]
    public async Task FullTest()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        int[] statuses = [StatusConstants.Blocked.Id, StatusConstants.InProgress.Id, StatusConstants.NotStarted.Id];
        int[] priorities = [PriorityConstants.High.Id, PriorityConstants.Normal.Id, PriorityConstants.Low.Id];
        int[] tenants =  [TenantConstants.Battlestar.Id, TenantConstants.Cylons.Id];

        var generator = new Faker<TaskCreateModel>()
            .RuleFor(p => p.Title, f => f.Lorem.Sentence())
            .RuleFor(p => p.Description, f => f.Lorem.Paragraph())
            .RuleFor(p => p.StatusId, f => f.Random.CollectionItem(statuses))
            .RuleFor(p => p.PriorityId, f => f.Random.CollectionItem(priorities))
            .RuleFor(p => p.TenantId, f => f.Random.CollectionItem(tenants));

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<TaskCreateModel, TaskReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<TaskReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (TaskReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBe(0);
        created.Title.Should().Be(createModel.Title);
        created.Description.Should().Be(createModel.Description);
        created.PriorityId.Should().Be(createModel.PriorityId);
        created.StatusId.Should().Be(createModel.StatusId);
        created.TenantId.Should().Be(createModel.TenantId);

        // get entity
        var getCommand = new GetEntity<int, TaskReadModel>(MockPrincipal.Default, created.Id);
        var getResult = await mediator.InvokeAsync<Result<TaskReadModel>>(getCommand);
        getResult.Should().NotBeNull();

        getResult.IsSuccess.Should().BeTrue();
        getResult.Value.Should().NotBeNull();

        var readModel = (TaskReadModel)getResult;
        readModel.Should().NotBeNull();
        readModel.Id.Should().Be(created.Id);
        readModel.Title.Should().Be(createModel.Title);
        readModel.Description.Should().Be(createModel.Description);

        // query entity
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new() { Name = nameof(TaskReadModel.Title) }]
        };

        var queryCommand = new QueryEntities<TaskReadModel>(MockPrincipal.Default, queryDefinition);

        var queryResult = await mediator.InvokeAsync<Result<QueryResult<TaskReadModel>>>(queryCommand);
        queryResult.Should().NotBeNull();

        var queryModel = (QueryResult<TaskReadModel>)queryResult;
        queryModel.Should().NotBeNull();
        queryModel.Data.Should().NotBeNull();
        queryModel.Total.Should().BeGreaterThan(0);

        // update entity
        TaskUpdateModel updateRequest = new()
        {
            StatusId = createModel.StatusId,
            PriorityId = createModel.PriorityId,
            TenantId = createModel.TenantId,
            Title = createModel.Title + " Updated",
            Description = createModel.Description + " Updated",
            StartDate = createModel.StartDate,
            DueDate = createModel.DueDate,
            CompleteDate = createModel.CompleteDate,
            AssignedId = createModel.AssignedId,
            IsDeleted = createModel.IsDeleted,
            RowVersion = created.RowVersion,
        };
        var updateCommand = new UpdateEntity<int, TaskUpdateModel, TaskReadModel>(MockPrincipal.Default, created.Id, updateRequest);

        var updateResult = await mediator.InvokeAsync<Result<TaskReadModel>>(updateCommand);
        updateResult.Should().NotBeNull();

        updateResult.IsSuccess.Should().BeTrue();
        updateResult.Value.Should().NotBeNull();

        var updatedModel = (TaskReadModel)updateResult;
        updatedModel.Should().NotBeNull();
        updatedModel.Id.Should().Be(created.Id);
        updatedModel.Title.Should().Be(updateRequest.Title);
        updatedModel.Description.Should().Be(updateRequest.Description);

        // delete entity
        var deleteCommand = new DeleteEntity<int, TaskReadModel>(MockPrincipal.Default, created.Id);
        var deleteResult = await mediator.InvokeAsync<Result<TaskReadModel>>(deleteCommand);
        deleteResult.Should().NotBeNull();
        deleteResult.IsSuccess.Should().BeTrue();
    }
}
