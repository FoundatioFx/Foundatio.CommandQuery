using System.Text.Json;

using AwesomeAssertions;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests.Results;

public class QueryResultTests
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void JsonSerialization_WithContinuationTokenAndTotal_SerializesCorrectly()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel>
        {
            Data = results,
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
        deserialized.Data.Should().HaveCount(1);
        deserialized.Data.First().Id.Should().Be(1);
        deserialized.Data.First().Name.Should().Be("Test1");
    }

    [Fact]
    public void JsonSerialization_WithNullContinuationTokenAndTotal_ExcludesNullProperties()
    {
        // Arrange
        var results = new List<TestModel>
        {
            new() { Id = 1, Name = "Test1" }
        };
        var queryResult = new QueryResult<TestModel> { Data = results };

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
        var queryResult = new QueryResult<TestModel>
        {
            Data = results,
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
        var queryResult = new QueryResult<TestModel>
        {
            Data = results,
            Total = 25
        };

        // Act
        var json = JsonSerializer.Serialize(queryResult);

        // Assert
        json.Should().Contain("\"total\":25");
        json.Should().NotContain("continuationToken");
    }

}
