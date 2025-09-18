using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Queries;

/// <summary>
/// Represents a sort specification for queries, including the property name and sort direction.
/// </summary>
[Equatable]
public partial class QuerySort
{
    /// <summary>
    /// Gets or sets the name of the property to sort by.
    /// </summary>
    /// <value>
    /// The name of the property to sort by.
    /// </value>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the sort order is descending.
    /// </summary>
    /// <value>
    /// <c>true</c> if the sort order is descending; otherwise, <c>false</c> for ascending order.
    /// </value>
    [JsonPropertyName("descending")]
    public bool Descending { get; set; }

    /// <summary>
    /// Returns a string representation of the sort specification.
    /// </summary>
    /// <returns>A string in the format "Name desc" for descending sorts or "Name" for ascending sorts.</returns>
    public override string ToString()
        => Descending ? $"{Name} desc" : Name;

    /// <summary>
    /// Parses the specified sort string into a <see cref="QuerySort"/> object.
    /// </summary>
    /// <param name="sortString">The sort string to parse. Supports formats like "Name", "Name desc", "Name:desc", "Name asc", or "Name:asc".</param>
    /// <returns>A <see cref="QuerySort"/> object if the parse was successful; otherwise, <c>null</c>.</returns>
    /// <remarks>
    /// The method supports both colon-separated and space-separated formats.
    /// If no direction is specified, ascending order is assumed.
    /// Direction matching is case-insensitive.
    /// </remarks>
    public static QuerySort? Parse(string? sortString)
    {
        if (string.IsNullOrWhiteSpace(sortString))
            return null;

        // support "Name desc" or "Name:desc"
        var parts = sortString.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length == 0)
            return null;

        var sort = new QuerySort { Name = parts[0] };

        // descending if starts with "desc"
        sort.Descending = parts.Length > 1 && parts[1].StartsWith("desc", StringComparison.OrdinalIgnoreCase);

        return sort;
    }
}
