using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Queries;

/// <summary>
/// Defines logical operators used in query groups for data-bound components.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<QueryLogic>))]
public enum QueryLogic
{
    /// <summary>
    /// Represents the logical "AND" operator for combining query rules.
    /// </summary>
    And = 0,

    /// <summary>
    /// Represents the logical "OR" operator for combining query rules.
    /// </summary>
    Or = 1
}
