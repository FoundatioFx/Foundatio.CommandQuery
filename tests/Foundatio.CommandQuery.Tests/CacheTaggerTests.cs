using AwesomeAssertions;

namespace Foundatio.CommandQuery.Tests;

public class CacheTaggerTests
{
    public CacheTaggerTests()
    {
        // Clear the internal cache before each test to ensure test isolation
        CacheTagger.ClearCache();
    }

    [Fact]
    public void SetTag_WithStringTag_SetsTagForType()
    {
        // Arrange
        const string expectedTag = "custom-tag";

        // Act
        CacheTagger.SetTag<TestModel>(expectedTag);

        // Assert
        var actualTag = CacheTagger.GetTag<TestModel>();
        actualTag.Should().Be(expectedTag);
    }

    [Fact]
    public void SetTag_WithNullTag_SetsNullTagForType()
    {
        // Act
        CacheTagger.SetTag<TestModel>(null);

        // Assert
        var actualTag = CacheTagger.GetTag<TestModel>();
        actualTag.Should().BeNull();
    }

    [Fact]
    public void SetTag_WithEntityType_SetsFullNameAsTag()
    {
        // Act
        CacheTagger.SetTag<TestModel, TestEntity>();

        // Assert
        var actualTag = CacheTagger.GetTag<TestModel>();
        actualTag.Should().Be(typeof(TestEntity).FullName);
    }

    [Fact]
    public void GetTag_WithoutSetTag_ReturnsTypeFullName()
    {
        // Act
        var tag = CacheTagger.GetTag<TestModel>();

        // Assert
        tag.Should().Be(typeof(TestModel).FullName);
    }

    [Fact]
    public void GetTag_WithSetTag_ReturnsCustomTag()
    {
        // Arrange
        const string customTag = "my-custom-tag";
        CacheTagger.SetTag<TestModel>(customTag);

        // Act
        var tag = CacheTagger.GetTag<TestModel>();

        // Assert
        tag.Should().Be(customTag);
    }

    [Fact]
    public void GetKey_WithDefaultDelimiter_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "test-bucket";
        const string value = "test-value";

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithCustomDelimiter_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "test-bucket";
        const string value = "test-value";
        const string delimiter = "-";

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value, delimiter);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}-{bucket}-{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithCustomTag_UsesCustomTag()
    {
        // Arrange
        const string customTag = "custom-model";
        const string bucket = "test-bucket";
        const string value = "test-value";

        CacheTagger.SetTag<TestModel>(customTag);

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value);

        // Assert
        var expectedKey = $"{customTag}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithNullTag_UsesTypeFullName()
    {
        // Arrange
        const string bucket = "test-bucket";
        const string value = "test-value";
        CacheTagger.SetTag<TestModel>(null);

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithIntegerValue_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "id";
        const int value = 123;

        // Act
        var key = CacheTagger.GetKey<TestModel, int>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithGuidValue_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "id";
        var value = Guid.NewGuid();

        // Act
        var key = CacheTagger.GetKey<TestModel, Guid>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void Buckets_Constants_HaveCorrectValues()
    {
        // Assert
        CacheTagger.Buckets.Identifier.Should().Be("id");
        CacheTagger.Buckets.Identifiers.Should().Be("ids");
        CacheTagger.Buckets.Query.Should().Be("query");
    }

    [Fact]
    public void SetTag_CalledMultipleTimes_OnlyFirstCallTakesEffect()
    {
        // Arrange
        const string firstTag = "first-tag";
        const string secondTag = "second-tag";

        // Act
        CacheTagger.SetTag<TestModel>(firstTag);
        CacheTagger.SetTag<TestModel>(secondTag); // This should be ignored

        // Assert
        var actualTag = CacheTagger.GetTag<TestModel>();
        actualTag.Should().Be(firstTag);
    }

    [Fact]
    public void CacheTagger_WithDifferentTypes_MaintainsSeparateTags()
    {
        // Arrange
        const string tag1 = "model1-tag";
        const string tag2 = "model2-tag";

        // Act
        CacheTagger.SetTag<TestModel>(tag1);
        CacheTagger.SetTag<AnotherTestModel>(tag2);

        // Assert
        CacheTagger.GetTag<TestModel>().Should().Be(tag1);
        CacheTagger.GetTag<AnotherTestModel>().Should().Be(tag2);
    }

    [Fact]
    public void GetKey_WithEmptyStringValues_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "test";
        const string value = "";

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithSpecialCharacters_GeneratesCorrectKey()
    {
        // Arrange
        const string bucket = "test-bucket";
        const string value = "value@with#special$chars";

        // Act
        var key = CacheTagger.GetKey<TestModel, string>(bucket, value);

        // Assert
        var expectedKey = $"{typeof(TestModel).FullName}.{bucket}.{value}";
        key.Should().Be(expectedKey);
    }

    [Fact]
    public void GetKey_WithBucketConstants_GeneratesCorrectKeys()
    {
        // Arrange
        const string value = "test-value";

        // Act & Assert
        var idKey = CacheTagger.GetKey<TestModel, string>(CacheTagger.Buckets.Identifier, value);
        var idsKey = CacheTagger.GetKey<TestModel, string>(CacheTagger.Buckets.Identifiers, value);
        var queryKey = CacheTagger.GetKey<TestModel, string>(CacheTagger.Buckets.Query, value);

        idKey.Should().Be($"{typeof(TestModel).FullName}.id.{value}");
        idsKey.Should().Be($"{typeof(TestModel).FullName}.ids.{value}");
        queryKey.Should().Be($"{typeof(TestModel).FullName}.query.{value}");
    }

    [Fact]
    public void ClearCache_ClearsAllTags()
    {
        // Arrange
        CacheTagger.SetTag<TestModel>("test-tag");
        CacheTagger.SetTag<AnotherTestModel>("another-tag");

        // Verify tags are set
        CacheTagger.GetTag<TestModel>().Should().Be("test-tag");
        CacheTagger.GetTag<AnotherTestModel>().Should().Be("another-tag");

        // Act
        CacheTagger.ClearCache();

        // Assert
        CacheTagger.GetTag<TestModel>().Should().Be(typeof(TestModel).FullName);
        CacheTagger.GetTag<AnotherTestModel>().Should().Be(typeof(AnotherTestModel).FullName);
    }

    // Test models for testing
    private class TestModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    private class AnotherTestModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string? Value { get; set; }
    }
}
