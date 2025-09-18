using AwesomeAssertions;

using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Tests.Queries;

public class QueryBuilderTests
{
    public class TestSearchModel : ISupportSearch
    {
        public static IEnumerable<string> SearchFields() => ["Name", "Description", "Tags"];
        public static string SortField() => "Name";
    }

    public class EmptySearchModel : ISupportSearch
    {
        public static IEnumerable<string> SearchFields() => [];
        public static string SortField() => string.Empty;
    }

    [Fact]
    public void Search_WithValidModelAndText_ReturnsValidQueryGroup()
    {
        // Act
        var result = QueryBuilder.Search<TestSearchModel>("test");

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.Or);
        result.Filters.Should().HaveCount(3);

        var filters = result.Filters.Cast<QueryFilter>().ToList();
        filters[0].Name.Should().Be("Name");
        filters[0].Value.Should().Be("test");
        filters[0].Operator.Should().Be(QueryOperators.Contains);

        filters[1].Name.Should().Be("Description");
        filters[1].Value.Should().Be("test");
        filters[1].Operator.Should().Be(QueryOperators.Contains);

        filters[2].Name.Should().Be("Tags");
        filters[2].Value.Should().Be("test");
        filters[2].Operator.Should().Be(QueryOperators.Contains);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Search_WithInvalidSearchText_ReturnsNull(string? searchText)
    {
        // Act
        var result = QueryBuilder.Search<TestSearchModel>(searchText!);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Search_WithEmptySearchFields_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Search<EmptySearchModel>("test");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Search_WithFieldsAndValidText_ReturnsValidQueryGroup()
    {
        // Arrange
        var fields = new[] { "Field1", "Field2" };

        // Act
        var result = QueryBuilder.Search(fields, "search text");

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.Or);
        result.Filters.Should().HaveCount(2);

        var filters = result.Filters.Cast<QueryFilter>().ToList();
        filters[0].Name.Should().Be("Field1");
        filters[0].Value.Should().Be("search text");
        filters[0].Operator.Should().Be(QueryOperators.Contains);

        filters[1].Name.Should().Be("Field2");
        filters[1].Value.Should().Be("search text");
        filters[1].Operator.Should().Be(QueryOperators.Contains);
    }

    [Fact]
    public void Search_WithNullFields_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Search(null!, "test");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Search_WithEmptyFields_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Search([], "test");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Sort_WithValidModel_ReturnsQuerySort()
    {
        // Act
        var result = QueryBuilder.Sort<TestSearchModel>();

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Name");
        result.Descending.Should().BeFalse();
    }

    [Fact]
    public void Sort_WithEmptyModelSortField_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Sort<EmptySearchModel>();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Sort_WithFieldAndAscending_ReturnsCorrectSort()
    {
        // Act
        var result = QueryBuilder.Sort("TestField", false);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestField");
        result.Descending.Should().BeFalse();
    }

    [Fact]
    public void Sort_WithFieldAndDescending_ReturnsCorrectSort()
    {
        // Act
        var result = QueryBuilder.Sort("TestField", true);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestField");
        result.Descending.Should().BeTrue();
    }

    [Theory]
    [InlineData("Name", false)]
    [InlineData("Name desc", true)]
    [InlineData("Name:desc", true)]
    [InlineData("Name asc", false)]
    [InlineData("Name:asc", false)]
    public void Sort_WithValidSortString_ReturnsCorrectSort(string sortString, bool expectedDescending)
    {
        // Act
        var result = QueryBuilder.Sort(sortString);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Name");
        result.Descending.Should().Be(expectedDescending);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Sort_WithInvalidSortString_ReturnsNull(string? sortString)
    {
        // Act
        var result = QueryBuilder.Sort(sortString);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Filter_WithValidParameters_ReturnsCorrectFilter()
    {
        // Act
        var result = QueryBuilder.Filter("TestField", "TestValue", QueryOperators.Contains);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestField");
        result.Value.Should().Be("TestValue");
        result.Operator.Should().Be(QueryOperators.Contains);
    }

    [Fact]
    public void Filter_WithDefaultOperator_UsesEqual()
    {
        // Act
        var result = QueryBuilder.Filter("TestField", "TestValue");

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestField");
        result.Value.Should().Be("TestValue");
        result.Operator.Should().Be(QueryOperators.Equal);
    }

    [Fact]
    public void Filter_WithNullValue_AllowsNullValue()
    {
        // Act
        var result = QueryBuilder.Filter("TestField", null);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("TestField");
        result.Value.Should().BeNull();
        result.Operator.Should().Be(QueryOperators.Equal);
    }

    [Fact]
    public void Group_WithValidFilters_ReturnsAndGroup()
    {
        // Arrange
        var filter1 = QueryBuilder.Filter("Field1", "Value1");
        var filter2 = QueryBuilder.Filter("Field2", "Value2");

        // Act
        var result = QueryBuilder.Group(filter1, filter2);

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.And);
        result.Filters.Should().HaveCount(2);
        result.Filters.Should().Contain(filter1);
        result.Filters.Should().Contain(filter2);
    }

    [Fact]
    public void Group_WithLogicAndValidFilters_ReturnsCorrectGroup()
    {
        // Arrange
        var filter1 = QueryBuilder.Filter("Field1", "Value1");
        var filter2 = QueryBuilder.Filter("Field2", "Value2");

        // Act
        var result = QueryBuilder.Group(QueryLogic.Or, filter1, filter2);

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.Or);
        result.Filters.Should().HaveCount(2);
        result.Filters.Should().Contain(filter1);
        result.Filters.Should().Contain(filter2);
    }

    [Fact]
    public void Group_WithSingleValidFilter_ReturnsGroup()
    {
        // Arrange
        var filter = QueryBuilder.Filter("Field1", "Value1");

        // Act
        var result = QueryBuilder.Group(filter);

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.And);
        result.Filters.Should().HaveCount(1);
        result.Filters.Should().Contain(filter);
    }

    [Fact]
    public void Group_WithSingleQueryGroup_ReturnsSameGroup()
    {
        // Arrange
        var innerFilter = QueryBuilder.Filter("Field1", "Value1");
        var innerGroup = QueryBuilder.Group(QueryLogic.Or, innerFilter);

        // Act
        var result = QueryBuilder.Group(innerGroup!);

        // Assert
        result.Should().BeSameAs(innerGroup);
    }

    [Fact]
    public void Group_WithInvalidFilters_ReturnsNull()
    {
        // Arrange - create invalid filters
        var invalidFilter1 = new QueryFilter { Name = "", Value = "test" };
        var invalidFilter2 = new QueryFilter { Name = null, Value = "test" };

        // Act
        var result = QueryBuilder.Group(invalidFilter1, invalidFilter2);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Group_WithMixedValidAndInvalidFilters_ReturnsGroupWithValidFiltersOnly()
    {
        // Arrange
        var validFilter = QueryBuilder.Filter("Field1", "Value1");
        var invalidFilter = new QueryFilter { Name = "", Value = "test" };

        // Act
        var result = QueryBuilder.Group(validFilter, invalidFilter);

        // Assert
        result.Should().NotBeNull();
        result!.Filters.Should().HaveCount(1);
        result.Filters.Should().Contain(validFilter);
    }

    [Fact]
    public void Group_WithNullFilters_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Group(null, null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Group_WithEmptyFilters_ReturnsNull()
    {
        // Act
        var result = QueryBuilder.Group();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Group_WithNestedGroups_ReturnsCorrectStructure()
    {
        // Arrange
        var filter1 = QueryBuilder.Filter("Field1", "Value1");
        var filter2 = QueryBuilder.Filter("Field2", "Value2");
        var innerGroup = QueryBuilder.Group(QueryLogic.Or, filter1, filter2);
        var filter3 = QueryBuilder.Filter("Field3", "Value3");

        // Act
        var result = QueryBuilder.Group(QueryLogic.And, innerGroup!, filter3);

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.And);
        result.Filters.Should().HaveCount(2);
        result.Filters.Should().Contain(innerGroup!);
        result.Filters.Should().Contain(filter3);
    }

    [Fact]
    public void Group_WithArrayOfFilters_ReturnsCorrectGroup()
    {
        // Arrange
        var filters = new QueryFilter[]
        {
            QueryBuilder.Filter("Field1", "Value1"),
            QueryBuilder.Filter("Field2", "Value2"),
            QueryBuilder.Filter("Field3", "Value3")
        };

        // Act
        var result = QueryBuilder.Group(filters);

        // Assert
        result.Should().NotBeNull();
        result!.Logic.Should().Be(QueryLogic.And);
        result.Filters.Should().HaveCount(3);
        result.Filters.Should().BeEquivalentTo(filters);
    }
}
