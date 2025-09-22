using System.Security.Claims;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Queries;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace Foundatio.CommandQuery.Endpoints;

/// <summary>
/// Provides a base class for defining RESTful query endpoints for an entity, including single, paged, and list queries.
/// </summary>
/// <typeparam name="TKey">The type of the entity key.</typeparam>
/// <typeparam name="TListModel">The type of the list model returned by queries.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned by single-entity queries.</typeparam>
public abstract class EntityQueryEndpointBase<TKey, TListModel, TReadModel> : IEndpointRoute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityQueryEndpointBase{TKey, TListModel, TReadModel}"/> class.
    /// </summary>
    /// <param name="loggerFactory">The logger factory to create an <see cref="ILogger"/> for this endpoint.</param>
    /// <param name="entityName">The name of the entity for this endpoint.</param>
    /// <param name="routePrefix">The route prefix for this endpoint. If not set, <paramref name="entityName"/> is used.</param>
    protected EntityQueryEndpointBase(ILoggerFactory loggerFactory, string entityName, string? routePrefix = null)
    {
        ArgumentNullException.ThrowIfNull(loggerFactory);
        ArgumentException.ThrowIfNullOrEmpty(entityName);

        Logger = loggerFactory.CreateLogger(GetType());

        EntityName = entityName;
        RoutePrefix = routePrefix ?? EntityName;
    }

    /// <summary>
    /// Gets the name of the entity for this endpoint.
    /// </summary>
    public string EntityName { get; }

    /// <summary>
    /// Gets the route prefix for this endpoint.
    /// </summary>
    public string RoutePrefix { get; }

    /// <summary>
    /// Gets the logger for this endpoint.
    /// </summary>
    protected ILogger Logger { get; }

    /// <inheritdoc/>
    public void AddRoutes(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup(RoutePrefix);

        MapGroup(group);
    }

    /// <summary>
    /// Maps the group of query endpoints for this entity, including single, paged, and list queries.
    /// </summary>
    /// <param name="group">The <see cref="RouteGroupBuilder"/> used to define the endpoint group.</param>
    protected virtual void MapGroup(RouteGroupBuilder group)
    {
        group
            .MapGet("{id}", GetEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithTags(EntityName)
            .WithName($"Get{EntityName}")
            .WithSummary("Get an entity by id")
            .WithDescription("Get an entity by id");

        group
            .MapGet("", GetQuery)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithTags(EntityName)
            .WithName($"Query{EntityName}")
            .WithSummary("Get entities by query")
            .WithDescription("Get entities by query");

        group
            .MapPost("", PostQuery)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .ProducesValidationProblem()
            .WithTags(EntityName)
            .WithName($"Query{EntityName}")
            .WithSummary("Get entities by query")
            .WithDescription("Get entities by query");
    }

    /// <summary>
    /// Retrieves a single entity by its identifier using the mediator service.
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/> to send the request to.</param>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="user">The current security claims principal.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>
    /// An awaitable task returning either <see cref="Ok{TReadModel}"/> with the entity or <see cref="ProblemHttpResult"/> on error.
    /// </returns>
    protected virtual async Task<Results<Ok<TReadModel>, ProblemHttpResult>> GetEntity(
        [FromServices] IMediator mediator,
        [FromRoute] TKey id,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new GetEntity<TKey, TReadModel>(user, id);

            var result = await mediator.InvokeAsync<Result<TReadModel>>(command, cancellationToken).ConfigureAwait(false);

            return TypedResults.Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error GetEntity: {ErrorMessage}", ex.Message);

            var details = ex.ToProblemDetails();
            return TypedResults.Problem(details);
        }
    }

    /// <summary>
    /// Retrieves entities using query parameters.
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/> to send the request to.</param>
    /// <param name="sort">The sort expression.</param>
    /// <param name="page">The page number for the query. The default is 1.</param>
    /// <param name="size">The size of the page for the query. The default is 20.</param>
    /// <param name="user">The current security claims principal.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>
    /// An awaitable task returning either <see cref="QueryResult{TListModel}"/> with the paged result or <see cref="ProblemHttpResult"/> on error.
    /// </returns>
    protected virtual async Task<Results<Ok<QueryResult<TListModel>>, ProblemHttpResult>> GetQuery(
        [FromServices] IMediator mediator,
        [FromQuery] string? sort = null,
        [FromQuery] int? page = 1,
        [FromQuery] int? size = 20,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var querySort = QuerySort.Parse(sort);
            var queryDefinition = new QueryDefinition
            {
                Sorts = [querySort],
                Page = page,
                PageSize = size
            };

            var command = new QueryEntities<TListModel>(user, queryDefinition);

            var result = await mediator.InvokeAsync<Result<QueryResult<TListModel>>>(command, cancellationToken).ConfigureAwait(false);

            return TypedResults.Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error GetQuery: {ErrorMessage}", ex.Message);

            var details = ex.ToProblemDetails();
            return TypedResults.Problem(details);
        }

    }

    /// <summary>
    /// Retrieves entities using a posted <see cref="QueryDefinition"/> object.
    /// </summary>
    /// <param name="mediator">The <see cref="IMediator"/> to send the request to.</param>
    /// <param name="query">The entity query specifying filter, sort, and pagination.</param>
    /// <param name="user">The current security claims principal.</param>
    /// <param name="cancellationToken">The request cancellation token.</param>
    /// <returns>
    /// An awaitable task returning either <see cref="QueryResult{TListModel}"/> with the paged result or <see cref="ProblemHttpResult"/> on error.
    /// </returns>
    protected virtual async Task<Results<Ok<QueryResult<TListModel>>, ProblemHttpResult>> PostQuery(
        [FromServices] IMediator mediator,
        [FromBody] QueryDefinition query,
        ClaimsPrincipal? user = default,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new QueryEntities<TListModel>(user, query);

            var result = await mediator.InvokeAsync<Result<QueryResult<TListModel>>>(command, cancellationToken).ConfigureAwait(false);

            return TypedResults.Ok(result.Value);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error PostQuery: {ErrorMessage}", ex.Message);

            var details = ex.ToProblemDetails();
            return TypedResults.Problem(details);
        }

    }
}
