using System.Text.Json;

using AwesomeAssertions;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests.Converters;

public class QueryFilterConverterTests
{
    [Fact]
    public void JsonSerialization_WithSimpleFilter_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "status",
            Value = "active",
            Operator = QueryOperators.Equal
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("status");
        deserializedFilter.Value.Should().Be("active");
        deserializedFilter.Operator.Should().Be(QueryOperators.Equal);
    }

    [Fact]
    public void JsonSerialization_WithFilterGroup_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "status", Value = "active", Operator = QueryOperators.Equal },
                new() { Name = "count", Value = 10, Operator = QueryOperators.GreaterThan }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(QueryLogic.And);
        deserializedFilter.Filters.Should().NotBeNull();
        deserializedFilter.Filters!.Count.Should().Be(2);

        var firstFilter = deserializedFilter.Filters[0];
        firstFilter.Name.Should().Be("status");
        firstFilter.Value.Should().Be("active");
        firstFilter.Operator.Should().Be(QueryOperators.Equal);

        var secondFilter = deserializedFilter.Filters[1];
        secondFilter.Name.Should().Be("count");
        secondFilter.Operator.Should().Be(QueryOperators.GreaterThan);
    }

    [Fact]
    public void JsonSerialization_WithNullValues_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "description",
            Operator = QueryOperators.IsNull
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("description");
        deserializedFilter.Value.Should().BeNull();
        deserializedFilter.Operator.Should().Be(QueryOperators.IsNull);
    }

    [Fact]
    public void JsonSerialization_WithNestedGroups_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.Or,
            Filters =
            [
                new() { Name = "status", Value = "active", Operator = QueryOperators.Equal },
                new()
                {
                    Logic = QueryLogic.And,
                    Filters =
                    [
                        new() { Name = "priority", Value = "high", Operator = QueryOperators.Equal },
                        new() { Name = "assignee", Operator = QueryOperators.IsNotNull }
                    ]
                }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(QueryLogic.Or);
        deserializedFilter.Filters.Should().NotBeNull();
        deserializedFilter.Filters!.Count.Should().Be(2);

        var nestedGroup = deserializedFilter.Filters[1];
        nestedGroup.Logic.Should().Be(QueryLogic.And);
        nestedGroup.Filters.Should().NotBeNull();
        nestedGroup.Filters!.Count.Should().Be(2);
        nestedGroup.Filters[0].Name.Should().Be("priority");
        nestedGroup.Filters[1].Name.Should().Be("assignee");
        nestedGroup.Filters[1].Operator.Should().Be(QueryOperators.IsNotNull);
    }

    [Fact]
    public void JsonSerialization_WithVariousValueTypes_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "name", Value = "test", Operator = QueryOperators.Contains },
                new() { Name = "count", Value = 42, Operator = QueryOperators.Equal },
                new() { Name = "score", Value = 85.5, Operator = QueryOperators.GreaterThan },
                new() { Name = "enabled", Value = true, Operator = QueryOperators.Equal }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Filters.Should().NotBeNull();
        deserializedFilter.Filters!.Count.Should().Be(4);

        deserializedFilter.Filters[0].Value.Should().Be("test");
        deserializedFilter.Filters[1].Value.Should().Be(42);
        deserializedFilter.Filters[2].Value.Should().Be(85.5);
        deserializedFilter.Filters[3].Value.Should().Be(true);
    }

    [Fact]
    public void JsonSerialization_WithStringArray_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "categories",
            Value = new[] { "technology", "science", "business" },
            Operator = QueryOperators.In
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("categories");
        deserializedFilter.Operator.Should().Be(QueryOperators.In);

        var deserializedArray = deserializedFilter.Value as string[];
        deserializedArray.Should().NotBeNull();
        deserializedArray.Should().BeEquivalentTo(new[] { "technology", "science", "business" });
    }

    [Fact]
    public void JsonSerialization_WithIntegerArray_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "ids",
            Value = new[] { 1, 2, 3, 5, 8 },
            Operator = QueryOperators.In
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("ids");
        deserializedFilter.Operator.Should().Be(QueryOperators.In);

        var deserializedArray = deserializedFilter.Value as int[];
        deserializedArray.Should().NotBeNull();
        deserializedArray.Should().BeEquivalentTo(new[] { 1, 2, 3, 5, 8 });
    }

    [Fact]
    public void JsonSerialization_WithMixedTypeArray_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "values",
            Value = new object[] { "text", 42, true, 3.14 },
            Operator = QueryOperators.In
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("values");
        deserializedFilter.Operator.Should().Be(QueryOperators.In);

        var deserializedArray = deserializedFilter.Value as object[];
        deserializedArray.Should().NotBeNull();
        deserializedArray.Should().BeEquivalentTo(new object[] { "text", 42, true, 3.14 });
    }

    [Fact]
    public void JsonSerialization_WithEmptyArray_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "tags",
            Value = Array.Empty<string>(),
            Operator = QueryOperators.In
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("tags");
        deserializedFilter.Operator.Should().Be(QueryOperators.In);

        var deserializedArray = deserializedFilter.Value as object[];
        deserializedArray.Should().NotBeNull();
        deserializedArray.Should().BeEmpty();
    }

    [Fact]
    public void JsonSerialization_WithDateTimeValue_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var testDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        var filter = new QueryFilter
        {
            Name = "createdDate",
            Value = testDate,
            Operator = QueryOperators.GreaterThan
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("createdDate");
        deserializedFilter.Operator.Should().Be(QueryOperators.GreaterThan);

        var parsed = DateTime.TryParse(deserializedFilter.Value?.ToString(), out var deserializedDate);
        parsed.Should().BeTrue();
    }

    [Fact]
    public void JsonSerialization_WithGuidValue_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var testGuid = Guid.NewGuid();
        var filter = new QueryFilter
        {
            Name = "userId",
            Value = testGuid,
            Operator = QueryOperators.Equal
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("userId");
        deserializedFilter.Operator.Should().Be(QueryOperators.Equal);

        var deserializedGuid = Guid.Parse(deserializedFilter.Value!.ToString()!);
        deserializedGuid.Should().Be(testGuid);
    }

    [Fact]
    public void JsonSerialization_WithDecimalValue_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "price",
            Value = 123.45m,
            Operator = QueryOperators.LessThanOrEqual
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Name.Should().Be("price");
        deserializedFilter.Operator.Should().Be(QueryOperators.LessThanOrEqual);

        var deserializedDecimal = JsonSerializer.Deserialize<decimal>(deserializedFilter.Value!.ToString()!);
        deserializedDecimal.Should().Be(123.45m);
    }

    [Fact]
    public void JsonSerialization_WithComplexNestedStructure_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "status", Value = "active", Operator = QueryOperators.Equal },
                new()
                {
                    Logic = QueryLogic.Or,
                    Filters =
                    [
                        new() { Name = "categories", Value = new[] { "tech", "science" }, Operator = QueryOperators.In },
                        new()
                        {
                            Logic = QueryLogic.And,
                            Filters =
                            [
                                new() { Name = "priority", Value = "high", Operator = QueryOperators.Equal },
                                new() { Name = "score", Value = 90, Operator = QueryOperators.GreaterThan }
                            ]
                        }
                    ]
                }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(QueryLogic.And);
        deserializedFilter.Filters.Should().HaveCount(2);

        var nestedOrGroup = deserializedFilter.Filters![1];
        nestedOrGroup.Logic.Should().Be(QueryLogic.Or);
        nestedOrGroup.Filters.Should().HaveCount(2);

        var categoriesFilter = nestedOrGroup.Filters![0];
        categoriesFilter.Name.Should().Be("categories");
        categoriesFilter.Operator.Should().Be(QueryOperators.In);

        var categoriesArray = categoriesFilter.Value as string[];
        categoriesArray.Should().NotBeNull();
        categoriesArray.Should().BeEquivalentTo(new[] { "tech", "science" });

        var deepestAndGroup = nestedOrGroup.Filters[1];
        deepestAndGroup.Logic.Should().Be(QueryLogic.And);
        deepestAndGroup.Filters.Should().HaveCount(2);
    }

    [Fact]
    public void JsonSerialization_WithAllQueryOperators_SerializesAndDeserializesCorrectly()
    {
        // Arrange
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
            QueryOperators.In,
            QueryOperators.NotIn,
            QueryOperators.IsNull,
            QueryOperators.IsNotNull,
            QueryOperators.Expression
        };

        foreach (var op in operators)
        {
            // Arrange
            var filter = new QueryFilter
            {
                Name = "testField",
                Value = op == QueryOperators.IsNull || op == QueryOperators.IsNotNull ? null : "testValue",
                Operator = op
            };

            // Act
            var json = JsonSerializer.Serialize(filter);
            var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

            // Assert
            deserializedFilter.Should().NotBeNull();
            deserializedFilter!.Operator.Should().Be(op);
        }
    }

    [Fact]
    public void JsonSerialization_WithEmptyFilterGroup_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters = []
        };

        // Act
        var json = JsonSerializer.Serialize(filter);
        var deserializedFilter = JsonSerializer.Deserialize<QueryFilter>(json);

        // Assert
        deserializedFilter.Should().NotBeNull();
        deserializedFilter!.Logic.Should().Be(QueryLogic.And);
        deserializedFilter.Filters.Should().NotBeNull();
        deserializedFilter.Filters!.Should().BeEmpty();
    }
}
