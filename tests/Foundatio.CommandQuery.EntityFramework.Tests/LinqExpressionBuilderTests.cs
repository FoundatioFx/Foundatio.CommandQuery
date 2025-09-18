using AwesomeAssertions;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

public class LinqExpressionBuilderTests
{
    #region Basic Functionality Tests

    [Fact]
    public void Build_WithNullQueryFilter_ReturnsEmptyExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();

        // Act
        builder.Build(null);

        // Assert
        builder.Expression.Should().BeEmpty();
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithEmptyFilter_ReturnsEmptyExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter();

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().BeEmpty();
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_ClearsExistingExpressionAndParameters()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter1 = new QueryFilter { Name = "Field1", Value = "value1" };
        var filter2 = new QueryFilter { Name = "Field2", Value = "value2" };

        // Act
        builder.Build(filter1);
        var firstExpression = builder.Expression;
        var firstParameters = builder.Parameters.ToList();

        builder.Build(filter2);

        // Assert
        builder.Expression.Should().NotBe(firstExpression);
        builder.Expression.Should().Be("Field2 == @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be("value2");
    }

    #endregion

    #region Standard Filter Tests

    [Fact]
    public void Build_WithEqualOperator_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Name",
            Value = "John",
            Operator = QueryOperators.Equal
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("Name == @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be("John");
    }

    [Fact]
    public void Build_WithNoOperator_DefaultsToEqual()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Age",
            Value = 25
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("Age == @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be(25);
    }

    [Theory]
    [InlineData(QueryOperators.NotEqual, "!=")]
    [InlineData(QueryOperators.GreaterThan, ">")]
    [InlineData(QueryOperators.GreaterThanOrEqual, ">=")]
    [InlineData(QueryOperators.LessThan, "<")]
    [InlineData(QueryOperators.LessThanOrEqual, "<=")]
    public void Build_WithComparisonOperators_GeneratesCorrectExpression(QueryOperators op, string expectedSymbol)
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Score",
            Value = 100,
            Operator = op
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be($"Score {expectedSymbol} @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be(100);
    }

    #endregion

    #region String Filter Tests

    [Theory]
    [InlineData(QueryOperators.Contains, "Name != NULL && Name.Contains(@0)")]
    [InlineData(QueryOperators.StartsWith, "Name != NULL && Name.StartsWith(@0)")]
    [InlineData(QueryOperators.EndsWith, "Name != NULL && Name.EndsWith(@0)")]
    public void Build_WithStringOperators_GeneratesCorrectExpression(QueryOperators op, string expectedExpression)
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Name",
            Value = "test",
            Operator = op
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be(expectedExpression);
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be("test");
    }

    [Theory]
    [InlineData(QueryOperators.NotContains, "Name != NULL && !Name.Contains(@0)")]
    [InlineData(QueryOperators.NotStartsWith, "Name != NULL && !Name.StartsWith(@0)")]
    [InlineData(QueryOperators.NotEndsWith, "Name != NULL && !Name.EndsWith(@0)")]
    public void Build_WithNegatedStringOperators_GeneratesCorrectExpression(QueryOperators op, string expectedExpression)
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Name",
            Value = "test",
            Operator = op
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be(expectedExpression);
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be("test");
    }

    #endregion

    #region Null Filter Tests

    [Fact]
    public void Build_WithIsNullOperator_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Description",
            Operator = QueryOperators.IsNull
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("Description == NULL");
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithIsNotNullOperator_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Description",
            Operator = QueryOperators.IsNotNull
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("Description != NULL");
        builder.Parameters.Should().BeEmpty();
    }

    #endregion

    #region In Filter Tests

    [Fact]
    public void Build_WithInOperator_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var expectedArray = new[] { "Active", "Pending" };
        var filter = new QueryFilter
        {
            Name = "Status",
            Value = expectedArray,
            Operator = QueryOperators.In
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("it.Status in @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().BeEquivalentTo(expectedArray);
    }

    [Fact]
    public void Build_WithNotInOperator_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var expectedArray = new[] { 1, 2, 3 };
        var filter = new QueryFilter
        {
            Name = "Category",
            Value = expectedArray,
            Operator = QueryOperators.NotIn
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("!it.Category in @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().BeEquivalentTo(expectedArray);
    }

    #endregion

    #region Expression Filter Tests

    [Fact]
    public void Build_WithExpressionOperator_GeneratesRawExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();

        var filter = new QueryFilter
        {
            Filters =
            [
                new QueryFilter
                {
                    Name = "PartnerId",
                    Value = 1000,
                },
                new QueryFilter
                {
                    Name = "Locations.Any(it.Id in @0)",
                    Value = new[] { 1, 2 },
                    Operator = QueryOperators.Expression
                }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("(PartnerId == @0 && Locations.Any(it.Id in @1))");
        builder.Parameters.Should().HaveCount(2);
        builder.Parameters[0].Should().Be(1000);
        builder.Parameters[1].Should().BeEquivalentTo(new[] { 1, 2 });
    }
    #endregion

    #region Group Filter Tests

    [Fact]
    public void Build_WithAndGroup_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new QueryFilter { Name = "Name", Value = "John", Operator = QueryOperators.Equal },
                new QueryFilter { Name = "Age", Value = 25, Operator = QueryOperators.GreaterThan }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("(Name == @0 && Age > @1)");
        builder.Parameters.Should().HaveCount(2);
        builder.Parameters[0].Should().Be("John");
        builder.Parameters[1].Should().Be(25);
    }

    [Fact]
    public void Build_WithOrGroup_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.Or,
            Filters =
            [
                new QueryFilter { Name = "Status", Value = "Active", Operator = QueryOperators.Equal },
                new QueryFilter { Name = "Status", Value = "Pending", Operator = QueryOperators.Equal }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("(Status == @0 || Status == @1)");
        builder.Parameters.Should().HaveCount(2);
        builder.Parameters[0].Should().Be("Active");
        builder.Parameters[1].Should().Be("Pending");
    }

    [Fact]
    public void Build_WithNestedGroups_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new QueryFilter { Name = "Active", Value = true, Operator = QueryOperators.Equal },
                new QueryFilter
                {
                    Logic = QueryLogic.Or,
                    Filters =
                    [
                        new QueryFilter { Name = "Type", Value = "Premium", Operator = QueryOperators.Equal },
                        new QueryFilter { Name = "Type", Value = "Standard", Operator = QueryOperators.Equal }
                    ]
                }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("(Active == @0 && (Type == @1 || Type == @2))");
        builder.Parameters.Should().HaveCount(3);
        builder.Parameters[0].Should().Be(true);
        builder.Parameters[1].Should().Be("Premium");
        builder.Parameters[2].Should().Be("Standard");
    }

    [Fact]
    public void Build_WithEmptyGroup_GeneratesEmptyExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters = []
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().BeEmpty();
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithSingleFilterInGroup_GeneratesExpressionWithParentheses()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new QueryFilter { Name = "Name", Value = "Test", Operator = QueryOperators.Contains }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("(Name != NULL && Name.Contains(@0))");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().Be("Test");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Build_WithNullOrEmptyFieldName_IgnoresFilter()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = null,
            Value = "test"
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().BeEmpty();
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithWhitespaceFieldName_IgnoresFilter()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "   ",
            Value = "test"
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().BeEmpty();
        builder.Parameters.Should().BeEmpty();
    }

    [Fact]
    public void Build_WithNullValue_StillGeneratesExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "Field",
            Value = null,
            Operator = QueryOperators.Equal
        };

        // Act
        builder.Build(filter);

        // Assert
        builder.Expression.Should().Be("Field == @0");
        builder.Parameters.Should().HaveCount(1);
        builder.Parameters[0].Should().BeNull();
    }

    [Fact]
    public void Build_WithComplexMixedFilters_GeneratesCorrectExpression()
    {
        // Arrange
        var builder = new LinqExpressionBuilder();
        var expectedArray = new[] { "A", "B" };
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters =
            [
                new QueryFilter { Name = "Name", Value = "John", Operator = QueryOperators.Contains },
                new QueryFilter { Name = "Age", Value = 18, Operator = QueryOperators.GreaterThanOrEqual },
                new QueryFilter { Name = "Status", Operator = QueryOperators.IsNotNull },
                new QueryFilter
                {
                    Logic = QueryLogic.Or,
                    Filters =
                    [
                        new QueryFilter { Name = "Category", Value = expectedArray, Operator = QueryOperators.In },
                        new QueryFilter { Name = "Priority", Value = "High", Operator = QueryOperators.Equal }
                    ]
                }
            ]
        };

        // Act
        builder.Build(filter);

        // Assert
        var expected = "(Name != NULL && Name.Contains(@0) && Age >= @1 && Status != NULL && (it.Category in @2 || Priority == @3))";
        builder.Expression.Should().Be(expected);
        builder.Parameters.Should().HaveCount(4);
        builder.Parameters[0].Should().Be("John");
        builder.Parameters[1].Should().Be(18);
        builder.Parameters[2].Should().BeEquivalentTo(expectedArray);
        builder.Parameters[3].Should().Be("High");
    }

    #endregion

    #region Custom Writer Tests

    // This test demonstrates custom writer registration but doesn't affect other tests
    // by using a unique operator and checking the registration only
    [Fact]
    public void RegisterWriter_WithCustomOperator_CanRegisterSuccessfully()
    {
        // Arrange
        var customOperator = (QueryOperators)999; // Custom operator not used elsewhere
        var customWriterCalled = false;

        // Register a custom writer that just sets a flag
        LinqExpressionBuilder.RegisterWriter(customOperator, (builder, parameters, filter) =>
        {
            customWriterCalled = true;
            builder.Append($"CUSTOM({filter.Name}, @{parameters.Count})");
            parameters.Add(filter.Value);
        });

        var linqBuilder = new LinqExpressionBuilder();
        var filter = new QueryFilter
        {
            Name = "TestField",
            Value = "TestValue",
            Operator = customOperator
        };

        // Act
        linqBuilder.Build(filter);

        // Assert
        customWriterCalled.Should().BeTrue();
        linqBuilder.Expression.Should().Be("CUSTOM(TestField, @0)");
        linqBuilder.Parameters.Should().HaveCount(1);
        linqBuilder.Parameters[0].Should().Be("TestValue");
    }

    #endregion
}
