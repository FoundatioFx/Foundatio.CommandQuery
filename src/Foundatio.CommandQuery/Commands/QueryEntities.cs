using System.Security.Claims;
using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Abstracts;
using Foundatio.CommandQuery.Queries;
using Foundatio.CommandQuery.Results;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// Represents a query for retrieving paged entities based on a <see cref="QueryDefinition"/>.
/// The result of the query will be of type <see cref="QueryResult{TReadModel}"/>.
/// </summary>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the query.</typeparam>
/// <remarks>
/// This query is typically used in a CQRS (Command Query Responsibility Segregation) pattern to retrieve entities
/// in a paginated format. The <see cref="QueryDefinition"/> allows filtering, sorting, and pagination criteria to be specified.
/// </remarks>
public record QueryEntities<TReadModel> : CacheableQuery
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryEntities{TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the query.</param>
    /// <param name="query">The <see cref="QueryDefinition"/> defining the filter, sort, and pagination criteria for the query.</param>
    public QueryEntities(ClaimsPrincipal? principal, QueryDefinition? query)
        : base(principal)
    {
        Query = query;
    }

    /// <summary>
    /// Gets the <see cref="QueryDefinition"/> defining the filter, sort, and pagination criteria for the query.
    /// </summary>
    [JsonPropertyName("query")]
    public QueryDefinition? Query { get; }

    /// <summary>
    /// Generates a cache key for the query based on the <see cref="QueryDefinition"/>.
    /// </summary>
    /// <returns>
    /// A string representing the cache key for the query.
    /// </returns>
    public override string GetCacheKey()
        => CacheTagger.GetKey<TReadModel, int>(CacheTagger.Buckets.Query, Query?.GetHashCode() ?? 0);

    /// <summary>
    /// Gets the cache tag associated with the <typeparamref name="TReadModel"/>.
    /// </summary>
    /// <returns>
    /// The cache tag for the <typeparamref name="TReadModel"/>, or <see langword="null"/> if no tag is available.
    /// </returns>
    public override string? GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
