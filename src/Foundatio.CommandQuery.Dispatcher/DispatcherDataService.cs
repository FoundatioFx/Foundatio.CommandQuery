using System.Security.Claims;

using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Dispatcher;

/// <summary>
/// Provides a data service for dispatching common data requests to a data store.
/// </summary>
/// <remarks>
/// This service acts as an abstraction for sending queries and commands to a dispatcher, enabling operations
/// such as retrieving, creating, updating, and deleting entities in a consistent manner.
/// </remarks>
public class DispatcherDataService : IDispatcherDataService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DispatcherDataService"/> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher used to send requests.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="dispatcher"/> is <see langword="null"/>.</exception>
    public DispatcherDataService(IDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        Dispatcher = dispatcher;
    }

    /// <inheritdoc/>
    public IDispatcher Dispatcher { get; }

    /// <inheritdoc/>
    public async ValueTask<TModel?> Get<TKey, TModel>(
        TKey id,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new GetEntity<TKey, TModel>(user, id);
        command.Cache(cacheTime);

        return await Dispatcher
            .Send<TModel>(command, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyList<TModel>> Get<TKey, TModel>(
        IEnumerable<TKey> ids,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new GetEntities<TKey, TModel>(user, [.. ids]);
        command.Cache(cacheTime);

        var result = await Dispatcher
            .Send<IReadOnlyList<TModel>>(command, cancellationToken)
            .ConfigureAwait(false);

        return result ?? [];
    }

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyList<TModel>> All<TModel>(
        string? sortField = null,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class
    {
        var sort = QuerySort.Parse(sortField);

        var query = new QueryDefinition { Sorts = [sort] };

        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new QueryEntities<TModel>(user, query);
        command.Cache(cacheTime);

        var result = await Dispatcher
            .Send<QueryResult<TModel>>(command, cancellationToken)
            .ConfigureAwait(false);

        return result?.Data ?? [];
    }

    /// <inheritdoc/>
    public async ValueTask<QueryResult<TModel>> Query<TModel>(
        QueryDefinition? query = null,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new QueryEntities<TModel>(user, query);
        command.Cache(cacheTime);

        var result = await Dispatcher
            .Send<QueryResult<TModel>>(command, cancellationToken)
            .ConfigureAwait(false);

        return result ?? new QueryResult<TModel>();
    }

    /// <inheritdoc/>
    public async ValueTask<QueryResult<TModel>> Search<TModel>(
        string searchText,
        QueryDefinition? query = null,
        CancellationToken cancellationToken = default)
        where TModel : class, ISupportSearch
    {
        query ??= new QueryDefinition();

        var searchFilter = QueryBuilder.Search(TModel.SearchFields(), searchText);
        var sort = new QuerySort { Name = TModel.SortField() };

        query.Filter = QueryBuilder.Group(query?.Filter, searchFilter);

        return await Query<TModel>(query, cancellationToken: cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask<TReadModel?> Save<TKey, TUpdateModel, TReadModel>(
        TKey id,
        TUpdateModel updateModel,
        CancellationToken cancellationToken = default)
        where TReadModel : class
        where TUpdateModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new UpdateEntity<TKey, TUpdateModel, TReadModel>(user, id, updateModel, true);

        return await Dispatcher
            .Send<TReadModel>(command, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async ValueTask<TReadModel?> Create<TCreateModel, TReadModel>(
        TCreateModel createModel,
        CancellationToken cancellationToken = default)
        where TReadModel : class
        where TCreateModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new CreateEntity<TCreateModel, TReadModel>(user, createModel);

        return await Dispatcher
            .Send<TReadModel>(command, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async ValueTask<TReadModel?> Update<TKey, TUpdateModel, TReadModel>(
        TKey id,
        TUpdateModel updateModel,
        CancellationToken cancellationToken = default)
        where TReadModel : class
        where TUpdateModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);

        var command = new UpdateEntity<TKey, TUpdateModel, TReadModel>(user, id, updateModel);

        return await Dispatcher
            .Send<TReadModel>(command, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async ValueTask<TReadModel?> Delete<TKey, TReadModel>(
        TKey id,
        CancellationToken cancellationToken = default)
        where TReadModel : class
    {
        var user = await GetUser(cancellationToken).ConfigureAwait(false);
        var command = new DeleteEntity<TKey, TReadModel>(user, id);

        return await Dispatcher
            .Send<TReadModel>(command, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ClaimsPrincipal?> GetUser(CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult<ClaimsPrincipal?>(default);
    }
}
