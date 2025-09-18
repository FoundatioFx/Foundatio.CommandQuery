using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Abstracts;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// Represents a query for retrieving multiple entities identified by a list of keys.
/// The result of the query will be a collection of type <typeparamref name="TReadModel"/>.
/// </summary>
/// <typeparam name="TKey">The type of the keys used to identify the entities.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the query.</typeparam>
/// <remarks>
/// This query is typically used in a CQRS (Command Query Responsibility Segregation) pattern to retrieve multiple entities
/// based on their unique identifiers. It supports caching to optimize repeated queries for the same set of entities.
/// </remarks>
public record GetEntities<TKey, TReadModel> : CacheableQuery
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetEntities{TKey, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the query.</param>
    /// <param name="ids">The list of identifiers for the entities to retrieve.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ids"/> is <see langword="null"/>.</exception>
    public GetEntities(ClaimsPrincipal? principal, [NotNull] IReadOnlyCollection<TKey> ids)
        : base(principal)
    {
        ArgumentNullException.ThrowIfNull(ids);

        Ids = ids;
    }

    /// <summary>
    /// Gets the list of identifiers for the entities to retrieve.
    /// </summary>
    /// <value>
    /// The list of identifiers for the entities to retrieve.
    /// </value>
    [NotNull]
    [JsonPropertyName("ids")]
    public IReadOnlyCollection<TKey> Ids { get; }

    /// <summary>
    /// Generates a cache key for the query based on the list of identifiers.
    /// </summary>
    /// <returns>
    /// A string representing the cache key for the query.
    /// </returns>
    public override string GetCacheKey()
    {
        var hash = new HashCode();

        foreach (var id in Ids)
            hash.Add(id);

        return CacheTagger.GetKey<TReadModel, int>(CacheTagger.Buckets.Identifiers, hash.ToHashCode());
    }

    /// <summary>
    /// Gets the cache tag associated with the <typeparamref name="TReadModel"/>.
    /// </summary>
    /// <returns>
    /// The cache tag for the <typeparamref name="TReadModel"/>, or <see langword="null"/> if no tag is available.
    /// </returns>
    public override string? GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
