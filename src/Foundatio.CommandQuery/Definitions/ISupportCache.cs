namespace Foundatio.CommandQuery.Definitions;

/// <summary>
/// An <see langword="interface"/> indicating the implemented type supports caching
/// </summary>
public interface ISupportCache
{
    /// <summary>
    /// Gets the cache tag for the type.
    /// </summary>
    /// <returns>The cache tag</returns>
    static abstract string CacheTag();
}
