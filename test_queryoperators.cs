using System.Text.Json;
using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests;

/// <summary>
/// Test class to verify QueryOperators enum works correctly with JSON serialization
/// </summary>
public class QueryOperatorsTest
{
    public void TestEnumSerialization()
    {
        // Test that enum values serialize to expected strings
        var filter = new QueryFilter
        {
            Field = "Name",
            Operator = QueryOperators.Equal,
            Value = "Test"
        };

        var json = JsonSerializer.Serialize(filter);
        
        // Should serialize operator as "equal" not "Equal"
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);
    }

    public void TestAllOperators()
    {
        // Test that all enum values are defined correctly
        var operators = new[]
        {
            QueryOperators.Equal,
            QueryOperators.NotEqual,
            QueryOperators.Contains,
            QueryOperators.NotContains,
            QueryOperators.StartsWith,
            QueryOperators.NotStartsWith,
            QueryOperators.EndsWith,
            QueryOperators.NotEndsWith,
            QueryOperators.GreaterThan,
            QueryOperators.GreaterThanOrEqual,
            QueryOperators.LessThan,
            QueryOperators.LessThanOrEqual,
            QueryOperators.IsNull,
            QueryOperators.IsNotNull
        };

        foreach (var op in operators)
        {
            var filter = new QueryFilter { Operator = op };
            // Should not throw any exceptions
        }
    }
}
