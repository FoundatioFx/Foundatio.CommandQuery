using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Abstracts;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// Represents a query for retrieving a single entity identified by a specific key.
/// The result of the query will be of type <typeparamref name="TReadModel"/>.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the query.</typeparam>
/// <remarks>
/// This query is typically used in a CQRS (Command Query Responsibility Segregation) pattern to retrieve a single entity
/// based on its unique identifier. It supports caching to optimize repeated queries for the same entity.
/// </remarks>
public record GetEntity<TKey, TReadModel> : CacheableQuery
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetEntity{TKey, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the query.</param>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <see langword="null"/>.</exception>
    public GetEntity(ClaimsPrincipal? principal, [NotNull] TKey id)
        : base(principal)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    /// <summary>
    /// Gets the identifier of the entity to retrieve.
    /// </summary>
    /// <value>
    /// The identifier of the entity to retrieve.
    /// </value>
    [NotNull]
    [JsonPropertyName("id")]
    public TKey Id { get; }

    /// <summary>
    /// Generates a cache key for the query based on the identifier.
    /// </summary>
    /// <returns>
    /// A string representing the cache key for the query.
    /// </returns>
    public override string GetCacheKey()
        => CacheTagger.GetKey<TReadModel, TKey>(CacheTagger.Buckets.Identifier, Id);

    /// <summary>
    /// Gets the cache tag associated with the <typeparamref name="TReadModel"/>.
    /// </summary>
    /// <returns>
    /// The cache tag for the <typeparamref name="TReadModel"/>, or <see langword="null"/> if no tag is available.
    /// </returns>
    public override string? GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
