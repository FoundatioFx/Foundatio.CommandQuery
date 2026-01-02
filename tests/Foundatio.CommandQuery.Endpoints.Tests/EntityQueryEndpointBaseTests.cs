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

public class EntityQueryEndpointBaseTests
{
    [Fact]
    public void Constructor_WithValidParameters_SetsProperties()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");

        endpoint.EntityName.Should().Be("TestQuery");
        endpoint.RoutePrefix.Should().Be("TestQuery");
    }

    [Fact]
    public void Constructor_WithCustomRoutePrefix_SetsRoutePrefix()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery", "api/test");

        endpoint.EntityName.Should().Be("TestQuery");
        endpoint.RoutePrefix.Should().Be("api/test");
    }

    [Fact]
    public void Constructor_WithNullLoggerFactory_ThrowsArgumentNullException()
    {
        var action = () => new TestEntityQueryEndpoint(null!, "TestQuery");

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullOrEmptyEntityName_ThrowsArgumentException()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

        var actionNull = () => new TestEntityQueryEndpoint(loggerFactory!, null!);
        var actionEmpty = () => new TestEntityQueryEndpoint(loggerFactory!, "");

        actionNull.Should().Throw<ArgumentException>();
        actionEmpty.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task GetEntity_WithValidId_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestGetEntity(mediator, "test-id", principal);

        var okResult = result.Result as Ok<QueryTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("test-id");
        okResult.Value.Name.Should().Be("Test Entity");
    }

    [Fact]
    public async Task GetEntity_WithNullUser_ReturnsOkResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");

        var result = await endpoint.TestGetEntity(mediator, "test-id", null);

        var okResult = result.Result as Ok<QueryTestReadModel>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Id.Should().Be("test-id");
    }

    [Fact]
    public async Task GetQuery_WithDefaultParameters_ReturnsQueryResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestGetQuery(mediator, null, 1, 20, principal);

        var okResult = result.Result as Ok<QueryResult<QueryTestListModel>>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Data.Should().HaveCount(2);
        okResult.Value.Total.Should().Be(2);
    }

    [Fact]
    public async Task GetQuery_WithSortParameter_ReturnsQueryResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestGetQuery(mediator, "name", 1, 10, principal);

        var okResult = result.Result as Ok<QueryResult<QueryTestListModel>>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Data.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetQuery_WithCustomPagination_ReturnsQueryResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var result = await endpoint.TestGetQuery(mediator, null, 2, 5, principal);

        var okResult = result.Result as Ok<QueryResult<QueryTestListModel>>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
    }

    [Fact]
    public async Task PostQuery_WithQueryDefinition_ReturnsQueryResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));

        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 10,
            Sorts = [new QuerySort { Name = "Name", Descending = false }]
        };

        var result = await endpoint.TestPostQuery(mediator, queryDefinition, principal);

        var okResult = result.Result as Ok<QueryResult<QueryTestListModel>>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
        okResult.Value.Data.Should().HaveCount(2);
        okResult.Value.Total.Should().Be(2);
    }

    [Fact]
    public async Task PostQuery_WithNullUser_ReturnsQueryResult()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        serviceCollection.AddMediator();
        serviceCollection.AddSingleton<TestEntityQueryHandler>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
        var mediator = serviceProvider.GetService<IMediator>();

        loggerFactory.Should().NotBeNull();
        mediator.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery");

        var queryDefinition = new QueryDefinition
        {
            Page = 1,
            PageSize = 5
        };

        var result = await endpoint.TestPostQuery(mediator, queryDefinition, null);

        var okResult = result.Result as Ok<QueryResult<QueryTestListModel>>;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().NotBeNull();
    }

    [Fact]
    public void AddRoutes_ConfiguresExpectedRoutes()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

        loggerFactory.Should().NotBeNull();

        var endpoint = new TestEntityQueryEndpoint(loggerFactory, "TestQuery", "api/test");

        // Create a minimal endpoint route builder for testing
        var app = WebApplication.CreateBuilder().Build();
        var routeBuilder = app as IEndpointRouteBuilder;

        // This should not throw
        var action = () => endpoint.AddRoutes(routeBuilder);
        action.Should().NotThrow();
    }
}

// Test implementation of EntityQueryEndpointBase
public class TestEntityQueryEndpoint : EntityQueryEndpointBase<string, QueryTestListModel, QueryTestReadModel>
{
    public TestEntityQueryEndpoint(ILoggerFactory loggerFactory, string entityName, string? routePrefix = null)
        : base(loggerFactory, entityName, routePrefix)
    {
    }

    public Task<Results<Ok<QueryTestReadModel>, ProblemHttpResult>> TestGetEntity(
        IMediator mediator,
        string id,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return GetEntity(mediator, id, user, cancellationToken);
    }

    public Task<Results<Ok<QueryResult<QueryTestListModel>>, ProblemHttpResult>> TestGetQuery(
        IMediator mediator,
        string? sort = null,
        int? page = 1,
        int? size = 20,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return GetQuery(mediator, sort, page, size, user, cancellationToken);
    }

    public Task<Results<Ok<QueryResult<QueryTestListModel>>, ProblemHttpResult>> TestPostQuery(
        IMediator mediator,
        QueryDefinition query,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        return PostQuery(mediator, query, user, cancellationToken);
    }
}

// Test models for query endpoint tests
public class QueryTestListModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class QueryTestReadModel : ReadModel<string>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

// Test handler for query endpoint tests
public class TestEntityQueryHandler
{
    public ValueTask<Result<QueryTestReadModel>> HandleAsync(GetEntity<string, QueryTestReadModel> command, CancellationToken cancellationToken = default)
    {
        var readModel = new QueryTestReadModel
        {
            Id = command.Id,
            Name = "Test Entity",
            Description = "A test entity for query endpoint testing"
        };

        var result = Result<QueryTestReadModel>.Success(readModel);
        return ValueTask.FromResult(result);
    }

    public ValueTask<Result<QueryResult<QueryTestListModel>>> HandleAsync(QueryEntities<QueryTestListModel> command, CancellationToken cancellationToken = default)
    {
        var entities = new List<QueryTestListModel>
        {
            new() { Id = "entity-1", Name = "Entity 1", Description = "First test entity" },
            new() { Id = "entity-2", Name = "Entity 2", Description = "Second test entity" }
        };

        var queryResult = new QueryResult<QueryTestListModel>
        {
            Data = entities,
            Total = entities.Count
        };

        var result = Result<QueryResult<QueryTestListModel>>.Success(queryResult);
        return ValueTask.FromResult(result);
    }
}
