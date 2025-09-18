using System.Text.Json;

using AwesomeAssertions;

using Foundatio.CommandQuery.Results;

namespace Foundatio.CommandQuery.Tests.Results;

public class QueryResultTests
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void Constructor_WithValidResults_SetsValueProperty()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" },
            new() { Id = 2, Name = "Test2" }
        };

        // Act
        var queryResult = new QueryResult<TestModel>(results);

        // Assert
        queryResult.Value.Should().BeEquivalentTo(results);
        queryResult.Value.Should().HaveCount(2);
        queryResult.Value.First().Id.Should().Be(1);
        queryResult.Value.First().Name.Should().Be("Test1");
    }

    [Fact]
    public void Constructor_WithEmptyResults_SetsEmptyValueProperty()
    {
        // Arrange
        var results = new List<TestModel>();

        // Act
        var queryResult = new QueryResult<TestModel>(results);

        // Assert
        queryResult.Value.Should().NotBeNull();
        queryResult.Value.Should().BeEmpty();
    }

    [Fact]
    public void JsonSerialization_WithContinuationTokenAndTotal_SerializesCorrectly()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel>(results)
        {
            ContinuationToken = "token-123",
            Total = 50
        };

        // Act
        var json = JsonSerializer.Serialize(queryResult);
        var deserialized = JsonSerializer.Deserialize<QueryResult<TestModel>>(json);

        // Assert
        json.Should().Contain("\"continuationToken\":\"token-123\"");
        json.Should().Contain("\"total\":50");

        deserialized.Should().NotBeNull();
        deserialized!.ContinuationToken.Should().Be("token-123");
        deserialized.Total.Should().Be(50);
        deserialized.Value.Should().HaveCount(1);
        deserialized.Value.First().Id.Should().Be(1);
        deserialized.Value.First().Name.Should().Be("Test1");
    }

    [Fact]
    public void JsonSerialization_WithNullContinuationTokenAndTotal_ExcludesNullProperties()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel>(results);

        // Act
        var json = JsonSerializer.Serialize(queryResult);

        // Assert
        json.Should().NotContain("continuationToken");
        json.Should().NotContain("total");
    }

    [Fact]
    public void JsonSerialization_WithContinuationTokenOnly_SerializesOnlyNonNullProperties()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel>(results)
        {
            ContinuationToken = "token-456"
        };

        // Act
        var json = JsonSerializer.Serialize(queryResult);

        // Assert
        json.Should().Contain("\"continuationToken\":\"token-456\"");
        json.Should().NotContain("total");
    }

    [Fact]
    public void JsonSerialization_WithTotalOnly_SerializesOnlyNonNullProperties()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel>(results)
        {
            Total = 25
        };

        // Act
        var json = JsonSerializer.Serialize(queryResult);

        // Assert
        json.Should().Contain("\"total\":25");
        json.Should().NotContain("continuationToken");
    }

}
