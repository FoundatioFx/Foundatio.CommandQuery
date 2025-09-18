using AwesomeAssertions;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests.Queries;

public class QueryFilterTests
{
    [Fact]
    public void Constructor_WithDefaults_CreatesEmptyFilter()
    {
        // Act
        var filter = new QueryFilter();

        // Assert
        filter.Name.Should().BeNull();
        filter.Value.Should().BeNull();
        filter.Operator.Should().BeNull();
        filter.Filters.Should().BeNull();
        filter.Logic.Should().BeNull();
    }

    [Fact]
    public void Name_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var filter = new QueryFilter();
        const string fieldName = "TestField";

        // Act
        filter.Name = fieldName;

        // Assert
        filter.Name.Should().Be(fieldName);
    }

    [Fact]
    public void Value_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var filter = new QueryFilter();
        const string testValue = "TestValue";

        // Act
        filter.Value = testValue;

        // Assert
        filter.Value.Should().Be(testValue);
    }

    [Fact]
    public void Value_Property_CanStoreNull()
    {
        // Arrange
        var filter = new QueryFilter();

        // Act
        filter.Value = null;

        // Assert
        filter.Value.Should().BeNull();
    }

    [Fact]
    public void Value_Property_CanStoreDifferentTypes()
    {
        // Arrange
        var filter = new QueryFilter();

        // Act & Assert
        filter.Value = "string";
        filter.Value.Should().Be("string");

        filter.Value = 42;
        filter.Value.Should().Be(42);

        filter.Value = DateTime.Now;
        filter.Value.Should().BeOfType<DateTime>();

        filter.Value = true;
        filter.Value.Should().Be(true);
    }

    [Fact]
    public void Operator_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var filter = new QueryFilter();

        // Act
        filter.Operator = QueryOperators.Contains;

        // Assert
        filter.Operator.Should().Be(QueryOperators.Contains);
    }

    [Fact]
    public void Filters_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var filter = new QueryFilter();
        var nestedFilters = new List<QueryFilter>
        {
            new() { Name = "Field1", Value = "Value1" },
            new() { Name = "Field2", Value = "Value2" }
        };

        // Act
        filter.Filters = nestedFilters;

        // Assert
        filter.Filters.Should().BeEquivalentTo(nestedFilters);
    }

    [Fact]
    public void Logic_Property_CanBeSetAndRetrieved()
    {
        // Arrange
        var filter = new QueryFilter();

        // Act
        filter.Logic = QueryLogic.Or;

        // Assert
        filter.Logic.Should().Be(QueryLogic.Or);
    }

    [Fact]
    public void IsGroup_WithNullFilters_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter { Filters = null };

        // Act
        var result = filter.IsGroup();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsGroup_WithEmptyFilters_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter { Filters = [] };

        // Act
        var result = filter.IsGroup();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsGroup_WithFilters_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Filters =
            [
                new() { Name = "Field1", Value = "Value1" }
            ]
        };

        // Act
        var result = filter.IsGroup();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithNullName_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = null,
            Value = "test",
            Operator = QueryOperators.Equal
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithEmptyName_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "",
            Value = "test",
            Operator = QueryOperators.Equal
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithWhitespaceName_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "   ",
            Value = "test",
            Operator = QueryOperators.Equal
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithValidNameAndValue_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = QueryOperators.Equal
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithValidNameAndNullValue_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = null,
            Operator = QueryOperators.Equal
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WithIsNullOperator_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = null,
            Operator = QueryOperators.IsNull
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithIsNotNullOperator_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = null,
            Operator = QueryOperators.IsNotNull
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_WithIsNullOperatorAndValue_ReturnsTrue()
    {
        // Arrange - value is ignored for IsNull operator
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = "SomeValue",
            Operator = QueryOperators.IsNull
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_GroupWithValidFilters_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field1", Value = "Value1", Operator = QueryOperators.Equal },
                new() { Name = "Field2", Value = "Value2", Operator = QueryOperators.Contains }
            ]
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_GroupWithNoValidFilters_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "", Value = "Value1" },
                new() { Name = null, Value = "Value2" }
            ]
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsValid_GroupWithMixedValidAndInvalidFilters_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.Or,
            Filters =
            [
                new() { Name = "", Value = "Value1" }, // Invalid
                new() { Name = "ValidField", Value = "Value2", Operator = QueryOperators.Equal } // Valid
            ]
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_NestedGroupStructure_ReturnsTrue()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field1", Value = "Value1", Operator = QueryOperators.Equal },
                new()
                {
                    Logic = QueryLogic.Or,
                    Filters =
                    [
                        new() { Name = "Field2", Value = "Value2", Operator = QueryOperators.Contains },
                        new() { Name = "Field3", Value = "Value3", Operator = QueryOperators.StartsWith }
                    ]
                }
            ]
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsValid_NestedGroupWithInvalidFilters_ReturnsFalse()
    {
        // Arrange
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new()
                {
                    Logic = QueryLogic.Or,
                    Filters =
                    [
                        new() { Name = "", Value = "Value1" }, // Invalid
                        new() { Name = null, Value = "Value2" } // Invalid
                    ]
                }
            ]
        };

        // Act
        var result = filter.IsValid();

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(QueryOperators.Equal)]
    [InlineData(QueryOperators.NotEqual)]
    [InlineData(QueryOperators.Contains)]
    [InlineData(QueryOperators.NotContains)]
    [InlineData(QueryOperators.StartsWith)]
    [InlineData(QueryOperators.NotStartsWith)]
    [InlineData(QueryOperators.EndsWith)]
    [InlineData(QueryOperators.NotEndsWith)]
    [InlineData(QueryOperators.GreaterThan)]
    [InlineData(QueryOperators.GreaterThanOrEqual)]
    [InlineData(QueryOperators.LessThan)]
    [InlineData(QueryOperators.LessThanOrEqual)]
    [InlineData(QueryOperators.In)]
    [InlineData(QueryOperators.NotIn)]
    [InlineData(QueryOperators.Expression)]
    public void IsValid_WithValueRequiringOperators_RequiresValue(QueryOperators queryOperator)
    {
        // Arrange
        var filterWithValue = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = queryOperator
        };

        var filterWithoutValue = new QueryFilter
        {
            Name = "TestField",
            Value = null,
            Operator = queryOperator
        };

        // Act & Assert
        filterWithValue.IsValid().Should().BeTrue();
        filterWithoutValue.IsValid().Should().BeFalse();
    }

    [Theory]
    [InlineData(QueryOperators.IsNull)]
    [InlineData(QueryOperators.IsNotNull)]
    public void IsValid_WithNullCheckOperators_DoesNotRequireValue(QueryOperators queryOperator)
    {
        // Arrange
        var filterWithValue = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = queryOperator
        };

        var filterWithoutValue = new QueryFilter
        {
            Name = "TestField",
            Value = null,
            Operator = queryOperator
        };

        // Act & Assert
        filterWithValue.IsValid().Should().BeTrue();
        filterWithoutValue.IsValid().Should().BeTrue();
    }

    [Fact]
    public void Equality_SameProperties_ReturnsTrue()
    {
        // Arrange
        var filter1 = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = QueryOperators.Equal
        };

        var filter2 = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = QueryOperators.Equal
        };

        // Act & Assert
        filter1.Should().Be(filter2);
    }

    [Fact]
    public void Equality_DifferentProperties_ReturnsFalse()
    {
        // Arrange
        var filter1 = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue1",
            Operator = QueryOperators.Equal
        };

        var filter2 = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue2",
            Operator = QueryOperators.Equal
        };

        // Act & Assert
        filter1.Should().NotBe(filter2);
    }

    [Fact]
    public void Equality_GroupsWithSameFilters_ReturnsTrue()
    {
        // Arrange
        var filter1 = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field1", Value = "Value1", Operator = QueryOperators.Equal },
                new() { Name = "Field2", Value = "Value2", Operator = QueryOperators.Contains }
            ]
        };

        var filter2 = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field1", Value = "Value1", Operator = QueryOperators.Equal },
                new() { Name = "Field2", Value = "Value2", Operator = QueryOperators.Contains }
            ]
        };

        // Act & Assert
        filter1.Should().Be(filter2);
    }

    [Fact]
    public void Equality_GroupsWithDifferentFilters_ReturnsFalse()
    {
        // Arrange
        var filter1 = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field1", Value = "Value1", Operator = QueryOperators.Equal }
            ]
        };

        var filter2 = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new() { Name = "Field2", Value = "Value2", Operator = QueryOperators.Contains }
            ]
        };

        // Act & Assert
        filter1.Should().NotBe(filter2);
    }
}
