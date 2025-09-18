using System.Text.Json;

using AwesomeAssertions;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests.Queries;

public class QueryDefinitionTests
{
    [Fact]
    public void JsonSerialization_WithEmptyDefinition_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition();

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Sorts.Should().BeNull();
        deserializedDefinition.Filter.Should().BeNull();
        deserializedDefinition.ContinuationToken.Should().BeNull();
        deserializedDefinition.Page.Should().BeNull();
        deserializedDefinition.PageSize.Should().BeNull();
    }

    [Fact]
    public void JsonSerialization_WithAllPropertiesSet_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "name", Descending = false },
                new QuerySort { Name = "createdDate", Descending = true }
            ],
            Filter = new QueryFilter
            {
                Name = "status",
                Value = "active",
                Operator = QueryOperators.Equal
            },
            ContinuationToken = "abc123token",
            Page = 2,
            PageSize = 50
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();

        deserializedDefinition!.Sorts.Should().NotBeNull();
        deserializedDefinition.Sorts!.Count.Should().Be(2);
        deserializedDefinition.Sorts[0].Name.Should().Be("name");
        deserializedDefinition.Sorts[0].Descending.Should().BeFalse();
        deserializedDefinition.Sorts[1].Name.Should().Be("createdDate");
        deserializedDefinition.Sorts[1].Descending.Should().BeTrue();

        deserializedDefinition.Filter.Should().NotBeNull();
        deserializedDefinition.Filter!.Name.Should().Be("status");
        deserializedDefinition.Filter.Value.Should().Be("active");
        deserializedDefinition.Filter.Operator.Should().Be(QueryOperators.Equal);

        deserializedDefinition.ContinuationToken.Should().Be("abc123token");
        deserializedDefinition.Page.Should().Be(2);
        deserializedDefinition.PageSize.Should().Be(50);
    }

    [Fact]
    public void JsonSerialization_WithSortsOnly_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "title", Descending = false },
                new QuerySort { Name = "priority", Descending = true },
                new QuerySort { Name = "updatedDate", Descending = true }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Sorts.Should().NotBeNull();
        deserializedDefinition.Sorts!.Count.Should().Be(3);

        deserializedDefinition.Sorts[0].Name.Should().Be("title");
        deserializedDefinition.Sorts[0].Descending.Should().BeFalse();

        deserializedDefinition.Sorts[1].Name.Should().Be("priority");
        deserializedDefinition.Sorts[1].Descending.Should().BeTrue();

        deserializedDefinition.Sorts[2].Name.Should().Be("updatedDate");
        deserializedDefinition.Sorts[2].Descending.Should().BeTrue();
    }

    [Fact]
    public void JsonSerialization_WithFilterOnly_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Filter = new QueryFilter
            {
                Logic = QueryLogic.And,
                Filters =
                [
                    new QueryFilter { Name = "category", Value = "tech", Operator = QueryOperators.Equal },
                    new QueryFilter { Name = "score", Value = 80, Operator = QueryOperators.GreaterThan }
                ]
            }
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Filter.Should().NotBeNull();
        deserializedDefinition.Filter!.Logic.Should().Be(QueryLogic.And);
        deserializedDefinition.Filter.Filters.Should().NotBeNull();
        deserializedDefinition.Filter.Filters!.Count.Should().Be(2);

        deserializedDefinition.Filter.Filters[0].Name.Should().Be("category");
        deserializedDefinition.Filter.Filters[0].Value.Should().Be("tech");
        deserializedDefinition.Filter.Filters[0].Operator.Should().Be(QueryOperators.Equal);

        deserializedDefinition.Filter.Filters[1].Name.Should().Be("score");
        deserializedDefinition.Filter.Filters[1].Value.Should().Be(80);
        deserializedDefinition.Filter.Filters[1].Operator.Should().Be(QueryOperators.GreaterThan);
    }

    [Fact]
    public void JsonSerialization_WithPaginationOnly_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Page = 5,
            PageSize = 25
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Page.Should().Be(5);
        deserializedDefinition.PageSize.Should().Be(25);
        deserializedDefinition.Sorts.Should().BeNull();
        deserializedDefinition.Filter.Should().BeNull();
        deserializedDefinition.ContinuationToken.Should().BeNull();
    }

    [Fact]
    public void JsonSerialization_WithContinuationTokenOnly_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            ContinuationToken = "eyJsYXN0SWQiOiIxMjM0NSIsImxhc3RWYWx1ZSI6ImFiY2RlIn0="
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.ContinuationToken.Should().Be("eyJsYXN0SWQiOiIxMjM0NSIsImxhc3RWYWx1ZSI6ImFiY2RlIn0=");
        deserializedDefinition.Sorts.Should().BeNull();
        deserializedDefinition.Filter.Should().BeNull();
        deserializedDefinition.Page.Should().BeNull();
        deserializedDefinition.PageSize.Should().BeNull();
    }

    [Fact]
    public void JsonSerialization_WithEmptyCollections_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Sorts = []
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Sorts.Should().NotBeNull();
        deserializedDefinition.Sorts!.Should().BeEmpty();
    }

    [Fact]
    public void JsonSerialization_WithComplexNestedFilter_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "relevance", Descending = true }
            ],
            Filter = new QueryFilter
            {
                Logic = QueryLogic.And,
                Filters =
                [
                    new QueryFilter { Name = "published", Value = true, Operator = QueryOperators.Equal },
                    new QueryFilter
                    {
                        Logic = QueryLogic.Or,
                        Filters =
                        [
                            new QueryFilter { Name = "categories", Value = new[] { "tech", "science" }, Operator = QueryOperators.In },
                            new QueryFilter { Name = "featured", Value = true, Operator = QueryOperators.Equal }
                        ]
                    }
                ]
            },
            Page = 1,
            PageSize = 20
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();

        // Verify sorts
        deserializedDefinition!.Sorts.Should().NotBeNull();
        deserializedDefinition.Sorts!.Count.Should().Be(1);
        deserializedDefinition.Sorts[0].Name.Should().Be("relevance");
        deserializedDefinition.Sorts[0].Descending.Should().BeTrue();

        // Verify complex filter structure
        deserializedDefinition.Filter.Should().NotBeNull();
        deserializedDefinition.Filter!.Logic.Should().Be(QueryLogic.And);
        deserializedDefinition.Filter.Filters.Should().NotBeNull();
        deserializedDefinition.Filter.Filters!.Count.Should().Be(2);

        var publishedFilter = deserializedDefinition.Filter.Filters[0];
        publishedFilter.Name.Should().Be("published");
        publishedFilter.Value.Should().Be(true);
        publishedFilter.Operator.Should().Be(QueryOperators.Equal);

        var orGroup = deserializedDefinition.Filter.Filters[1];
        orGroup.Logic.Should().Be(QueryLogic.Or);
        orGroup.Filters.Should().NotBeNull();
        orGroup.Filters!.Count.Should().Be(2);

        var categoriesFilter = orGroup.Filters[0];
        categoriesFilter.Name.Should().Be("categories");
        categoriesFilter.Operator.Should().Be(QueryOperators.In);
        var categoriesArray = categoriesFilter.Value as string[];
        categoriesArray.Should().NotBeNull();
        categoriesArray.Should().BeEquivalentTo(new[] { "tech", "science" });

        var featuredFilter = orGroup.Filters[1];
        featuredFilter.Name.Should().Be("featured");
        featuredFilter.Value.Should().Be(true);
        featuredFilter.Operator.Should().Be(QueryOperators.Equal);

        // Verify pagination
        deserializedDefinition.Page.Should().Be(1);
        deserializedDefinition.PageSize.Should().Be(20);
    }

    [Fact]
    public void JsonSerialization_WithZeroPagination_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Page = 0,
            PageSize = 0
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Page.Should().Be(0);
        deserializedDefinition.PageSize.Should().Be(0);
    }

    [Fact]
    public void JsonSerialization_WithNegativePagination_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Page = -1,
            PageSize = -10
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Page.Should().Be(-1);
        deserializedDefinition.PageSize.Should().Be(-10);
    }

    [Fact]
    public void JsonSerialization_WithLargePaginationValues_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Page = int.MaxValue,
            PageSize = int.MaxValue
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Page.Should().Be(int.MaxValue);
        deserializedDefinition.PageSize.Should().Be(int.MaxValue);
    }

    [Fact]
    public void JsonSerialization_WithLongContinuationToken_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var longToken = new string('a', 1000);
        var definition = new QueryDefinition
        {
            ContinuationToken = longToken
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.ContinuationToken.Should().Be(longToken);
        deserializedDefinition.ContinuationToken!.Length.Should().Be(1000);
    }

    [Fact]
    public void JsonSerialization_WithSpecialCharactersInContinuationToken_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var specialToken = "token/with+special=chars&symbols%20encoded";
        var definition = new QueryDefinition
        {
            ContinuationToken = specialToken
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.ContinuationToken.Should().Be(specialToken);
    }

    [Fact]
    public void JsonSerialization_WithMultipleSortsVariousDirections_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "priority", Descending = true },
                new QuerySort { Name = "title", Descending = false },
                new QuerySort { Name = "createdDate", Descending = true },
                new QuerySort { Name = "id", Descending = false }
            ]
        };

        // Act
        var json = JsonSerializer.Serialize(definition);
        var deserializedDefinition = JsonSerializer.Deserialize<QueryDefinition>(json);

        // Assert
        deserializedDefinition.Should().NotBeNull();
        deserializedDefinition!.Sorts.Should().NotBeNull();
        deserializedDefinition.Sorts!.Count.Should().Be(4);

        var sorts = deserializedDefinition.Sorts;
        sorts[0].Name.Should().Be("priority");
        sorts[0].Descending.Should().BeTrue();

        sorts[1].Name.Should().Be("title");
        sorts[1].Descending.Should().BeFalse();

        sorts[2].Name.Should().Be("createdDate");
        sorts[2].Descending.Should().BeTrue();

        sorts[3].Name.Should().Be("id");
        sorts[3].Descending.Should().BeFalse();
    }

    [Fact]
    public void JsonSerialization_PropertyNames_UseCamelCaseNaming()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            PageSize = 10,
            ContinuationToken = "test"
        };

        // Act
        var json = JsonSerializer.Serialize(definition);

        // Assert
        json.Should().Contain("\"pageSize\"");
        json.Should().Contain("\"continuationToken\"");
        json.Should().NotContain("\"PageSize\"");
        json.Should().NotContain("\"ContinuationToken\"");
    }

    [Fact]
    public void JsonSerialization_NullProperties_AreNotSerialized()
    {
        // Arrange
        var definition = new QueryDefinition
        {
            Page = 1 // Only set one property
        };

        // Act
        var json = JsonSerializer.Serialize(definition);

        // Assert
        json.Should().Contain("\"page\"");
        json.Should().NotContain("\"sorts\"");
        json.Should().NotContain("\"filter\"");
        json.Should().NotContain("\"continuationToken\"");
        json.Should().NotContain("\"pageSize\"");
    }

    [Fact]
    public void Equality_WithIdenticalProperties_ReturnsTrue()
    {
        // Arrange
        var definition1 = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "name", Descending = false }
            ],
            Filter = new QueryFilter { Name = "status", Value = "active", Operator = QueryOperators.Equal },
            Page = 1,
            PageSize = 20,
            ContinuationToken = "token123"
        };

        var definition2 = new QueryDefinition
        {
            Sorts =
            [
                new QuerySort { Name = "name", Descending = false }
            ],
            Filter = new QueryFilter { Name = "status", Value = "active", Operator = QueryOperators.Equal },
            Page = 1,
            PageSize = 20,
            ContinuationToken = "token123"
        };

        // Act & Assert
        definition1.Should().Be(definition2);
    }

    [Fact]
    public void Equality_WithDifferentProperties_ReturnsFalse()
    {
        // Arrange
        var definition1 = new QueryDefinition
        {
            Page = 1,
            PageSize = 20
        };

        var definition2 = new QueryDefinition
        {
            Page = 2,
            PageSize = 20
        };

        // Act & Assert
        definition1.Should().NotBe(definition2);
    }
}
