using System.Security.Claims;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Models;
using Foundatio.CommandQuery.Queries;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Foundatio.CommandQuery.Endpoints.Tests;

public class EntityCommandEndpointBaseTests
{
    [Fact]
    public void Constructor_WithValidParameters_SetsProperties()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");

        endpoint.EntityName.Should().Be("TestCommand");
        endpoint.RoutePrefix.Should().Be("TestCommand");
    }

    [Fact]
    public void Constructor_WithCustomRoutePrefix_SetsRoutePrefix()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand", "api/test");

        endpoint.EntityName.Should().Be("TestCommand");
        endpoint.RoutePrefix.Should().Be("api/test");
    }

    [Fact]
    public void Constructor_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var action = () => new TestEntityCommandEndpoint(null!, "TestCommand");

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullOrEmptyEntityName_ThrowsArgumentException()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

        var actionNull = () => new TestEntityCommandEndpoint(loggerFactory!, null!);
        var actionEmpty = () => new TestEntityCommandEndpoint(loggerFactory!, "");

        actionNull.Should().Throw<ArgumentException>();
        actionEmpty.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task GetUpdateQuery_WithValidId_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestGetUpdateQuery(mediator, "test-id", principal);

        var okResult = result.Result as Ok<CommandTestUpdateModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Name.Should().Be("Update Entity");
    }

    [Fact]
    public async Task GetUpdateQuery_WithNullUser_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");

        var result = await endpoint.TestGetUpdateQuery(mediator, "test-id", null);

        var okResult = result.Result as Ok<CommandTestUpdateModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateCommand_WithValidModel_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var createModel = new CommandTestCreateModel
        {
            Id = "new-entity-123",
            Name = "New Entity",
            Description = "A new test entity"
        };

        var result = await endpoint.TestCreateCommand(mediator, createModel, principal);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("new-entity-123");
        okResult.Value.Name.Should().Be("New Entity");
        okResult.Value.Description.Should().Be("A new test entity");
    }

    [Fact]
    public async Task CreateCommand_WithNullUser_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");

        var createModel = new CommandTestCreateModel
        {
            Id = "anonymous-entity",
            Name = "Anonymous Entity",
            Description = "Created by anonymous user"
        };

        var result = await endpoint.TestCreateCommand(mediator, createModel, null);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Name.Should().Be("Anonymous Entity");
    }

    [Fact]
    public async Task UpdateCommand_WithValidModel_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var updateModel = new CommandTestUpdateModel
        {
            Name = "Updated Entity",
            Description = "An updated test entity",
            RowVersion = 1
        };

        var result = await endpoint.TestUpdateCommand(mediator, "update-id", updateModel, principal);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("update-id");
        okResult.Value.Name.Should().Be("Updated Entity");
        okResult.Value.Description.Should().Be("An updated test entity");
    }

    [Fact]
    public async Task UpdateCommand_WithNullUser_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");

        var updateModel = new CommandTestUpdateModel
        {
            Name = "Anonymous Update",
            Description = "Updated by anonymous user"
        };

        var result = await endpoint.TestUpdateCommand(mediator, "anonymous-update", updateModel, null);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Name.Should().Be("Anonymous Update");
    }

    [Fact]
    public async Task DeleteCommand_WithValidId_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestDeleteCommand(mediator, "delete-id", principal);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("delete-id");
        okResult.Value.Name.Should().Be("Deleted Entity");
    }

    [Fact]
    public async Task DeleteCommand_WithNullUser_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");

        var result = await endpoint.TestDeleteCommand(mediator, "anonymous-delete", null);

        var okResult = result.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("anonymous-delete");
    }

    [Fact]
    public void AddRoutes_ConfiguresExpectedRoutes()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand", "api/test");

        // Create a minimal endpoint route builder for testing
        var app = WebApplication.CreateBuilder().Build();
        var routeBuilder = app as IEndpointRouteBuilder;

        // This should not throw
        var action = () => endpoint.AddRoutes(routeBuilder);
        action.Should().NotThrow();
    }

    [Fact]
    public async Task InheritedQueryMethods_WorkCorrectly()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityCommandHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityCommandEndpoint(loggerFactory, "TestCommand");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        // Test inherited GetEntity method
        var getResult = await endpoint.TestGetEntity(mediator, "inherited-test", principal);

        var okResult = getResult.Result as Ok<CommandTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("inherited-test");
    }
}

// Test implementation of EntityCommandEndpointBase
public class TestEntityCommandEndpoint : EntityCommandEndpointBase<string, CommandTestListModel, CommandTestReadModel, CommandTestCreateModel, CommandTestUpdateModel>
{
    public TestEntityCommandEndpoint(ILoggerFactory loggerFactory, string entityName, string? routePrefix = null)
        : base(loggerFactory, entityName, routePrefix)
    {
    }

    public Task<Results<Ok<CommandTestUpdateModel>, ProblemHttpResult>> TestGetUpdateQuery(
        IMediator mediator,
        string id,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return GetUpdateQuery(mediator, id, user, cancellationToken);
    }

    public Task<Results<Ok<CommandTestReadModel>, ProblemHttpResult>> TestCreateCommand(
        IMediator mediator,
        CommandTestCreateModel createModel,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return CreateCommand(mediator, createModel, user, cancellationToken);
    }

    public Task<Results<Ok<CommandTestReadModel>, ProblemHttpResult>> TestUpdateCommand(
        IMediator mediator,
        string id,
        CommandTestUpdateModel updateModel,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return UpdateCommand(mediator, id, updateModel, user, cancellationToken);
    }

    public Task<Results<Ok<CommandTestReadModel>, ProblemHttpResult>> TestDeleteCommand(
        IMediator mediator,
        string id,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return DeleteCommand(mediator, id, user, cancellationToken);
    }

    // Expose inherited query methods for testing
    public Task<Results<Ok<CommandTestReadModel>, ProblemHttpResult>> TestGetEntity(
        IMediator mediator,
        string id,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return GetEntity(mediator, id, user, cancellationToken);
    }

    public Task<Results<Ok<QueryResult<CommandTestListModel>>, ProblemHttpResult>> TestGetQuery(
        IMediator mediator,
        string? sort = null,
        int? page = 1,
        int? size = 20,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return GetQuery(mediator, sort, page, size, user, cancellationToken);
    }
}

// Test models for command endpoint tests
public class CommandTestListModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class CommandTestReadModel : ReadModel<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class CommandTestCreateModel : CreateModel<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class CommandTestUpdateModel : UpdateModel
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

// Test handler for command endpoint tests
public class TestEntityCommandHandler
{
    public ValueTask<Result<CommandTestUpdateModel>> HandleAsync(GetEntity<string, CommandTestUpdateModel> command, CancellationToken cancellationToken = default)
    {
        var updateModel = new CommandTestUpdateModel
        {
            Name = "Update Entity",
            Description = "An entity ready for updating",
            RowVersion = 1
        };

        var result = Result<CommandTestUpdateModel>.Success(updateModel);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Result<CommandTestReadModel>> HandleAsync(CreateEntity<CommandTestCreateModel, CommandTestReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new CommandTestReadModel
        {
            Id = command.Model.Id,
            Name = command.Model.Name,
            Description = command.Model.Description,
            Created = command.Model.Created,
            CreatedBy = command.Model.CreatedBy,
            Updated = command.Model.Updated,
            UpdatedBy = command.Model.UpdatedBy
        };

        var result = Result<CommandTestReadModel>.Success(readModel);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Result<CommandTestReadModel>> HandleAsync(UpdateEntity<string, CommandTestUpdateModel, CommandTestReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new CommandTestReadModel
        {
            Id = command.Id,
            Name = command.Model.Name,
            Description = command.Model.Description,
            Updated = command.Model.Updated,
            UpdatedBy = command.Model.UpdatedBy,
            RowVersion = command.Model.RowVersion
        };

        var result = Result<CommandTestReadModel>.Success(readModel);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Result<CommandTestReadModel>> HandleAsync(DeleteEntity<string, CommandTestReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new CommandTestReadModel
        {
            Id = command.Id,
            Name = "Deleted Entity",
            Description = "This entity was deleted"
        };

        var result = Result<CommandTestReadModel>.Success(readModel);
        return ValueTask.FromResult(result);
    }

    // Inherited query handlers
    public ValueTask<Result<CommandTestReadModel>> HandleAsync(GetEntity<string, CommandTestReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new CommandTestReadModel
        {
            Id = command.Id,
            Name = "Test Entity",
            Description = "A test entity from inherited method"
        };

        var result = Result<CommandTestReadModel>.Success(readModel);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Result<QueryResult<CommandTestListModel>>> HandleAsync(QueryEntities<CommandTestListModel> command, CancellationToken cancellationToken = default)
    {
        var entities = new List<CommandTestListModel>
        {
            new() { Id = "entity-1", Name = "Entity 1", Description = "First test entity" },
            new() { Id = "entity-2", Name = "Entity 2", Description = "Second test entity" }
        };

        var queryResult = new QueryResult<CommandTestListModel>
        {
            Data = entities,
            Total = entities.Count
        };

        var result = Result<QueryResult<CommandTestListModel>>.Success(queryResult);
        return ValueTask.FromResult(result);
    }
}
