using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Converters;

namespace Foundatio.CommandQuery.Queries;

/// <summary>
/// Represents a single filter condition or group for queries in data-bound components.
/// Can function as either an individual filter with name, operator, and value, or as a group containing nested filters.
/// Used to create filter expressions that can be combined to build complex query structures.
/// </summary>
[Equatable]
[JsonConverter(typeof(QueryFilterConverter))]
public partial class QueryFilter
{
    /// <summary>
    /// Gets or sets the name of the field to filter on.
    /// This should correspond to a property or field name in the target data structure.
    /// Required for individual filters, not used for groups.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the value to compare against the field using the specified operator.
    /// The value type should be compatible with the field type and operator being used.
    /// For <see cref="QueryOperators.IsNull"/> and <see cref="QueryOperators.IsNotNull"/> operators, this value is ignored.
    /// Not used for groups.
    /// </summary>
    [JsonPropertyName("value")]
    public object? Value { get; set; }

    /// <summary>
    /// Gets or sets the operator used for the filter comparison.
    /// Supports various operators like equality, comparison, string matching, and null checks.
    /// When not explicitly set and <see cref="Name"/> is provided, defaults to <see cref="QueryOperators.Equal"/>.
    /// Not used for groups.
    /// </summary>
    /// <value>The operator to use when comparing the field value against the filter value.</value>
    [JsonPropertyName("operator")]
    [JsonConverter(typeof(JsonStringEnumConverter<QueryOperators>))]
    public QueryOperators? Operator { get; set; }

    /// <summary>
    /// Gets or sets the collection of filters and/or subgroups contained in this group.
    /// When populated, this filter acts as a group containing nested filters.
    /// This enables building complex nested query structures with arbitrary depth.
    /// </summary>
    [SequenceEquality]
    [JsonPropertyName("filters")]
    public List<QueryFilter>? Filters { get; set; }

    /// <summary>
    /// Gets or sets the logical operator used to combine the filters in this group.
    /// Determines how the filters within this group are logically combined during evaluation.
    /// When not explicitly set and <see cref="Filters"/> is not empty, defaults to <see cref="QueryLogic.And"/>.
    /// </summary>
    /// <value>The logical operator, typically <see cref="QueryLogic.And"/> or <see cref="QueryLogic.Or"/>.</value>
    [JsonPropertyName("logic")]
    [JsonConverter(typeof(JsonStringEnumConverter<QueryLogic>))]
    public QueryLogic? Logic { get; set; }


    /// <summary>
    /// Determines whether this query filter represents a group containing nested filters.
    /// A filter is considered a group if it has a non-empty Filters collection.
    /// </summary>
    /// <returns><c>true</c> if the filter contains nested filters; otherwise, <c>false</c>.</returns>
    public bool IsGroup()
    {
        return Filters is not null && Filters.Count > 0;
    }

    /// <summary>
    /// Determines whether this query filter is valid and can be used for filtering operations.
    /// A filter is considered valid if it represents a group with at least one valid nested filter,
    /// or if it has a non-empty field name and either uses a null-check operator or has a non-null value for comparison operators.
    /// </summary>
    /// <returns><c>true</c> if the filter is a valid group or has a valid field name and appropriate value for its operator; otherwise, <c>false</c>.</returns>
    public bool IsValid()
    {
        // Groups are valid if they contain at least one valid filter
        if (IsGroup())
            return Filters!.Any(f => f.IsValid());

        // Individual filters require a field name
        if (string.IsNullOrWhiteSpace(Name))
            return false;

        // Operators that don't require a value
        if (Operator is QueryOperators.IsNull or QueryOperators.IsNotNull)
            return true;

        // All other operators require a value
        return Value is not null;
    }

}
