using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Queries;

[Equatable]
public partial class QueryDefinition
{
    /// <summary>
    /// Gets or sets the list of sort expressions to apply to the query.
    /// </summary>
    [SequenceEquality]
    [JsonPropertyName("sorts")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<QuerySort>? Sorts { get; set; }

    /// <summary>
    /// Gets or sets the filter to apply to the query.
    /// </summary>
    [JsonPropertyName("filter")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QueryFilter? Filter { get; set; }

    /// <summary>
    /// Gets the continuation token for retrieving the next page of results.
    /// </summary>
    [JsonPropertyName("continuationToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContinuationToken { get; set; }

    /// <summary>
    /// Gets or sets the page number for the query.
    /// </summary>
    /// <value>
    /// The page number for the query. The default value is 1.
    /// </value>
    [JsonPropertyName("page")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Page { get; set; }

    /// <summary>
    /// Gets or sets the size of the page for the query.
    /// </summary>
    /// <value>
    /// The size of the page for the query. The default value is 20.
    /// </value>
    [JsonPropertyName("pageSize")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PageSize { get; set; }
}
