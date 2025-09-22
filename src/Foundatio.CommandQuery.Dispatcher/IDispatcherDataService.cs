using System.Security.Claims;

using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Dispatcher;

/// <summary>
/// Defines a service for dispatching common data requests to a data store.
/// </summary>
public interface IDispatcherDataService
{
    /// <summary>
    /// Gets the dispatcher used to send requests.
    /// </summary>
    IDispatcher Dispatcher { get; }

    /// <summary>
    /// Retrieves a model for the specified identifier key from the data store.
    /// </summary>
    /// <typeparam name="TKey">Type of the identifier key.</typeparam>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <param name="id">Identifier key.</param>
    /// <param name="cacheTime">Optional cache duration for the result.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the model for the specified identifier key, or null if not found.</returns>
    ValueTask<TModel?> Get<TKey, TModel>(
        TKey id,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class;

    /// <summary>
    /// Retrieves a list of models for the specified identifier keys from the data store.
    /// </summary>
    /// <typeparam name="TKey">Type of the identifier keys.</typeparam>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <param name="ids">Collection of identifier keys.</param>
    /// <param name="cacheTime">Optional cache duration for the result.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns a read-only collection of models for the specified keys.</returns>
    ValueTask<IReadOnlyList<TModel>> Get<TKey, TModel>(
        IEnumerable<TKey> ids,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class;

    /// <summary>
    /// Retrieves all models of the specified type from the data store.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <param name="sortField">Optional field or property name to sort by.</param>
    /// <param name="cacheTime">Optional cache duration for the result.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns a read-only collection of all models.</returns>
    ValueTask<IReadOnlyList<TModel>> All<TModel>(
        string? sortField = null,
        TimeSpan? cacheTime = null,
        CancellationToken cancellationToken = default)
        where TModel : class;

    /// <summary>
    /// Retrieves a page of models based on the specified query definition.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <param name="query">Query definition for filtering, sorting, and paging.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns a paged query result for the model type.</returns>
    ValueTask<QueryResult<TModel>> Query<TModel>(
        QueryDefinition? query = null,
        CancellationToken cancellationToken = default)
        where TModel : class;

    /// <summary>
    /// Searches the data store for models based on the specified search text and optional filter. <typeparamref name="TModel"/> must implement <see cref="ISupportSearch"/>.
    /// </summary>
    /// <typeparam name="TModel">Type of the model.</typeparam>
    /// <param name="searchText">Text to search for.</param>
    /// <param name="query">Optional additional filter for search.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns an enumerable of matching models.</returns>
    ValueTask<QueryResult<TModel>> Search<TModel>(
        string searchText,
        QueryDefinition? query = null,
        CancellationToken cancellationToken = default)
        where TModel : class, ISupportSearch;

    /// <summary>
    /// Saves the specified update model in the data store for the given identifier. Creates or updates as needed.
    /// </summary>
    /// <typeparam name="TKey">Type of the identifier key.</typeparam>
    /// <typeparam name="TUpdateModel">Type of the update model.</typeparam>
    /// <typeparam name="TReadModel">Type of the read model.</typeparam>
    /// <param name="id">Identifier to apply the update to.</param>
    /// <param name="updateModel">Update model to apply.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the updated or created read model, or null if not found.</returns>
    ValueTask<TReadModel?> Save<TKey, TUpdateModel, TReadModel>(
        TKey id,
        TUpdateModel updateModel,
        CancellationToken cancellationToken = default)
        where TUpdateModel : class where TReadModel : class;

    /// <summary>
    /// Creates the specified model in the data store.
    /// </summary>
    /// <typeparam name="TCreateModel">Type of the create model.</typeparam>
    /// <typeparam name="TReadModel">Type of the read model.</typeparam>
    /// <param name="createModel">Model to create.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the created read model, or null if creation failed.</returns>
    ValueTask<TReadModel?> Create<TCreateModel, TReadModel>(
        TCreateModel createModel,
        CancellationToken cancellationToken = default)
        where TCreateModel : class
        where TReadModel : class;

    /// <summary>
    /// Updates the specified model in the data store for the given identifier.
    /// </summary>
    /// <typeparam name="TKey">Type of the identifier key.</typeparam>
    /// <typeparam name="TUpdateModel">Type of the update model.</typeparam>
    /// <typeparam name="TReadModel">Type of the read model.</typeparam>
    /// <param name="id">Identifier to apply the update to.</param>
    /// <param name="updateModel">Model to update.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the updated read model, or null if not found.</returns>
    ValueTask<TReadModel?> Update<TKey, TUpdateModel, TReadModel>(
        TKey id,
        TUpdateModel updateModel,
        CancellationToken cancellationToken = default)
        where TUpdateModel : class where TReadModel : class;

    /// <summary>
    /// Deletes the model with the specified identifier from the data store.
    /// </summary>
    /// <typeparam name="TKey">Type of the identifier key.</typeparam>
    /// <typeparam name="TReadModel">Type of the read model.</typeparam>
    /// <param name="id">Identifier to delete.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the deleted read model, or null if not found.</returns>
    ValueTask<TReadModel?> Delete<TKey, TReadModel>(
        TKey id,
        CancellationToken cancellationToken = default) where TReadModel : class;

    /// <summary>
    /// Gets the current user from the request context.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A task that returns the current user principal, or null if unavailable.</returns>
    ValueTask<ClaimsPrincipal?> GetUser(CancellationToken cancellationToken = default);
}
