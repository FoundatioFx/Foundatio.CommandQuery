using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(QueryDefinition))]
[JsonSerializable(typeof(QuerySort))]
[JsonSerializable(typeof(List<QuerySort>))]
[JsonSerializable(typeof(QueryFilter))]
[JsonSerializable(typeof(List<QueryFilter>))]
public partial class JsonContext : JsonSerializerContext
{
}
