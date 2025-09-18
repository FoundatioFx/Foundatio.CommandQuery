using System.Text.Json.Serialization;

using Foundatio.Mediator;

namespace Foundatio.CommandQuery.Results;

/// <summary>
/// A result for an entity query.
/// </summary>
/// <typeparam name="TReadModel">The type of the read model.</typeparam>
public class QueryResult<TReadModel> : Result<IReadOnlyList<TReadModel>>
{
    public QueryResult(IReadOnlyList<TReadModel> results)
        : base(results)
    {
    }

    /// <summary>
    /// Gets or sets the continuation token for retrieving the next page of results.
    /// </summary>
    /// <value>
    /// A string token that can be used in subsequent queries to fetch the next set of results,
    /// or <see langword="null"/> if there are no more results.
    /// </value>
    [JsonPropertyName("continuationToken")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContinuationToken { get; set; }

    /// <summary>
    /// The total number of the results for the query.
    /// </summary>
    [JsonPropertyName("total")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Total { get; set; }
}
