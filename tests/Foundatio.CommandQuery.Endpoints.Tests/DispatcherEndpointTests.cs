using System.Security.Claims;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Dispatcher;
using Foundatio.CommandQuery.Models;
using Foundatio.CommandQuery.Queries;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Foundatio.CommandQuery.Endpoints.Tests;

public class DispatcherEndpointTests
{
    [Fact]
    public async Task DispatchRequest_TestRequest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();

        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var request = new TestRequest { Name = "Bob" };
        var dispatchRequest = DispatchRequest.Create(request);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());
        var endpoint = new TestDispatcherEndpoint(logger, options);
        endpoint.Should().NotBeNull();

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be("Hello, Bob!");
    }

    [Fact]
    public async Task DispatchRequest_CreateEntityCommand()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var createModel = new TestEntityCreateModel
        {
            Id = "test-entity-123",
            Name = "Test Entity",
            Description = "A test entity for validation"
        };

        var createCommand = new CreateEntity<TestEntityCreateModel, TestEntityReadModel>(principal, createModel);
        var dispatchRequest = DispatchRequest.Create(createCommand);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var readModel = okResult.Value.Should().BeOfType<TestEntityReadModel>().Subject;
        readModel.Id.Should().Be("test-entity-123");
        readModel.Name.Should().Be("Test Entity");
        readModel.Description.Should().Be("A test entity for validation");
    }

    [Fact]
    public async Task DispatchRequest_UpdateEntityCommand()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var updateModel = new TestEntityUpdateModel
        {
            Name = "Updated Entity",
            Description = "An updated test entity"
        };

        var updateCommand = new UpdateEntity<string, TestEntityUpdateModel, TestEntityReadModel>(principal, "test-entity-456", updateModel, upsert: false);
        var dispatchRequest = DispatchRequest.Create(updateCommand);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var readModel = okResult.Value.Should().BeOfType<TestEntityReadModel>().Subject;
        readModel.Id.Should().Be("test-entity-456");
        readModel.Name.Should().Be("Updated Entity");
        readModel.Description.Should().Be("An updated test entity");
    }

    [Fact]
    public async Task DispatchRequest_DeleteEntityCommand()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var deleteCommand = new DeleteEntity<string, TestEntityReadModel>(principal, "test-entity-789");
        var dispatchRequest = DispatchRequest.Create(deleteCommand);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var readModel = okResult.Value.Should().BeOfType<TestEntityReadModel>().Subject;
        readModel.Id.Should().Be("test-entity-789");
        readModel.Name.Should().Be("Deleted Entity");
    }

    [Fact]
    public async Task DispatchRequest_GetEntityCommand()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var getCommand = new GetEntity<string, TestEntityReadModel>(principal, "test-entity-get");
        var dispatchRequest = DispatchRequest.Create(getCommand);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var readModel = okResult.Value.Should().BeOfType<TestEntityReadModel>().Subject;
        readModel.Id.Should().Be("test-entity-get");
        readModel.Name.Should().Be("Retrieved Entity");
    }

    [Fact]
    public async Task DispatchRequest_QueryEntitiesCommand()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new QuerySort { Name = "Name", Descending = false }]
        };

        var queryCommand = new QueryEntities<TestEntityReadModel>(principal, queryDefinition);
        var dispatchRequest = DispatchRequest.Create(queryCommand);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);

        var queryResult = okResult.Value.Should().BeOfType<QueryResult<TestEntityReadModel>>().Subject;
        queryResult.Data.Should().HaveCount(2);
        queryResult.Total.Should().Be(2);
        queryResult.Data.First().Name.Should().Be("Entity 1");
        queryResult.Data.Last().Name.Should().Be("Entity 2");
    }

    [Fact]
    public async Task DispatchRequest_WithNullUser()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestRequestHandler>();

        var service = serviceCollection.BuildServiceProvider();

        var request = new TestRequest { Name = "Anonymous" };
        var dispatchRequest = DispatchRequest.Create(request);

        var logger = service.GetService<ILogger<DispatcherEndpoint>>();
        logger.Should().NotBeNull();

        var options = Options.Create(new DispatcherOptions());

        var endpoint = new TestDispatcherEndpoint(logger, options);

        var mediator = service.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var result = await endpoint.TestSend(dispatchRequest, mediator, user: null);
        result.Should().NotBeNull();

        var okResult = result as Microsoft.AspNetCore.Http.HttpResults.Ok<object>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be("Hello, Anonymous!");
    }
}

public class TestDispatcherEndpoint(ILogger<DispatcherEndpoint> logger, IOptions<DispatcherOptions> dispatcherOptions)
    : DispatcherEndpoint(logger, dispatcherOptions)
{
    public Task<IResult> TestSend(
        DispatchRequest dispatchRequest,
        IMediator mediator,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return Send(dispatchRequest, mediator, user, cancellationToken);
    }
}

// Test models and requests
public class TestRequest
{
    public string Name { get; set; } = null!;
}

public class TestEntityCreateModel : EntityCreateModel<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class TestEntityUpdateModel : EntityUpdateModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class TestEntityReadModel : EntityReadModel<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

// Test handlers
public class TestRequestHandler
{
    public ValueTask<string> HandleAsync(TestRequest message, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult($"Hello, {message.Name}!");
    }

    public ValueTask<TestEntityReadModel> HandleAsync(CreateEntity<TestEntityCreateModel, TestEntityReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new TestEntityReadModel
        {
            Id = command.Model.Id,
            Name = command.Model.Name,
            Description = command.Model.Description,
            Created = command.Model.Created,
            CreatedBy = command.Model.CreatedBy,
            Updated = command.Model.Updated,
            UpdatedBy = command.Model.UpdatedBy
        };

        return ValueTask.FromResult(readModel);
    }

    public ValueTask<TestEntityReadModel> HandleAsync(UpdateEntity<string, TestEntityUpdateModel, TestEntityReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new TestEntityReadModel
        {
            Id = command.Id,
            Name = command.Model.Name,
            Description = command.Model.Description,
            Updated = command.Model.Updated,
            UpdatedBy = command.Model.UpdatedBy,
            RowVersion = command.Model.RowVersion
        };

        return ValueTask.FromResult(readModel);
    }

    public ValueTask<TestEntityReadModel> HandleAsync(DeleteEntity<string, TestEntityReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new TestEntityReadModel
        {
            Id = command.Id,
            Name = "Deleted Entity",
            Description = "This entity was deleted"
        };

        return ValueTask.FromResult(readModel);
    }

    public ValueTask<TestEntityReadModel> HandleAsync(GetEntity<string, TestEntityReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new TestEntityReadModel
        {
            Id = command.Id,
            Name = "Retrieved Entity",
            Description = "This entity was retrieved"
        };

        return ValueTask.FromResult(readModel);
    }

    public ValueTask<QueryResult<TestEntityReadModel>> HandleAsync(QueryEntities<TestEntityReadModel> command, CancellationToken cancellationToken = default)
    {
        var entities = new List<TestEntityReadModel>
        {
            new() { Id = "entity-1", Name = "Entity 1", Description = "First entity" },
            new() { Id = "entity-2", Name = "Entity 2", Description = "Second entity" }
        };

        var result = new QueryResult<TestEntityReadModel>
        {
            Data = entities,
            Total = entities.Count
        };

        return ValueTask.FromResult(result);
    }
}
