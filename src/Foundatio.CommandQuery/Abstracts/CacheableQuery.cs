using System.Security.Claims;

using Foundatio.CommandQuery.Definitions;

namespace Foundatio.CommandQuery.Abstracts;

/// <summary>
/// Represents a base class for cacheable queries that use a specified <see cref="ClaimsPrincipal"/> for user context.
/// </summary>
/// <remarks>
/// This class provides support for cache key generation, cache tagging, and cache expiration policies.
/// It is intended for use in scenarios where query results can be cached and associated with a user principal.
/// </remarks>
public abstract record CacheableQuery : PrincipalCommand, ICacheResult
{
    private TimeSpan? _expiration;

    /// <summary>
    /// Initializes a new instance of the <see cref="CacheableQuery"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user for whom the query is executed.</param>
    protected CacheableQuery(ClaimsPrincipal? principal) : base(principal)
    {
    }


    /// <summary>
    /// Gets the cache key for this query instance.
    /// </summary>
    /// <returns>A string representing the cache key.</returns>
    public abstract string GetCacheKey();

    /// <summary>
    /// Gets the cache tag for this query instance.
    /// </summary>
    /// <returns>A string representing the cache tag, or <see langword="null"/> if not set.</returns>
    public virtual string? GetCacheTag() => null;

    /// <summary>
    /// Determines whether this query is cacheable based on the expiration settings.
    /// </summary>
    /// <returns>
    ///   <see langword="true"/> if either absolute or expiration is set; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsCacheable() => _expiration.HasValue;

    /// <summary>
    /// Sets the expiration for the cache entry associated with this query.
    /// </summary>
    /// <param name="expiration">The expiration time span, or <see langword="null"/> to unset.</param>
    public void Cache(TimeSpan? expiration)
    {
        _expiration = expiration;
    }

    /// <summary>
    /// Gets the expiration time span for the cache entry.
    /// </summary>
    /// <returns>
    /// The expiration as a <see cref="TimeSpan"/> if set; otherwise, <see langword="null"/>.
    /// </returns>
    TimeSpan? ICacheResult.Expiration()
    {
        return _expiration;
    }
}
