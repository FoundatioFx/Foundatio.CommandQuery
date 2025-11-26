using System.Linq.Dynamic.Core;

using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

public class QueryExtensionsTests
{
    #region Test Data Classes

    public class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public string? Category { get; set; }
        public int Priority { get; set; }
    }

    public class TestReadModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string? Status { get; set; }
    }

    public class TestMapper : IMapper
    {
        public TDestination? Map<TSource, TDestination>(TSource? source)
        {
            if (source is null) return default;

            if (typeof(TSource) == typeof(TestEntity) && typeof(TDestination) == typeof(TestReadModel))
            {
                var entity = source as TestEntity;
                var model = new TestReadModel
                {
                    Id = entity!.Id,
                    Name = entity.Name,
                    Age = entity.Age,
                    Status = entity.Status
                };
                return (TDestination)(object)model;
            }

            throw new NotSupportedException($"Mapping from {typeof(TSource)} to {typeof(TDestination)} is not supported");
        }

        public void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TDestination> ProjectTo<TSource, TDestination>(IQueryable<TSource> source)
        {
            if (typeof(TSource) == typeof(TestEntity) && typeof(TDestination) == typeof(TestReadModel))
            {
                var query = source as IQueryable<TestEntity>;
                var projected = query!.Select(e => new TestReadModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Status = e.Status
                });
                return (IQueryable<TDestination>)projected;
            }

            throw new NotSupportedException($"Projection from {typeof(TSource)} to {typeof(TDestination)} is not supported");
        }
    }

    #endregion

    #region Sort Tests - Single Sort

    [Fact]
    public void Sort_WithNullSort_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Charlie", Age = 25 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30 },
            new TestEntity { Id = 3, Name = "Bob", Age = 20 }
        };
        var query = data.AsQueryable();

        // Act
        var result = query.Sort((QuerySort?)null);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Sort_WithValidSort_AppliesSorting()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Charlie", Age = 25 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30 },
            new TestEntity { Id = 3, Name = "Bob", Age = 20 }
        };
        var query = data.AsQueryable();
        var sort = new QuerySort { Name = "Name", Descending = false };

        // Act
        var result = query.Sort(sort).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Alice");
        result[1].Name.Should().Be("Bob");
        result[2].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Sort_WithDescendingSort_AppliesCorrectSorting()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Charlie", Age = 25 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30 },
            new TestEntity { Id = 3, Name = "Bob", Age = 20 }
        };
        var query = data.AsQueryable();
        var sort = new QuerySort { Name = "Age", Descending = true };

        // Act
        var result = query.Sort(sort).ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Age.Should().Be(30);
        result[1].Age.Should().Be(25);
        result[2].Age.Should().Be(20);
    }

    #endregion

    #region Sort Tests - Multiple Sorts

    [Fact]
    public void Sort_WithNullSorts_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Charlie", Age = 25 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30 }
        };
        var query = data.AsQueryable();

        // Act
        var result = query.Sort((IEnumerable<QuerySort>?)null);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Sort_WithEmptySorts_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Charlie", Age = 25 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30 }
        };
        var query = data.AsQueryable();

        // Act
        var result = query.Sort(Array.Empty<QuerySort>());

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Sort_WithMultipleSorts_AppliesInCorrectOrder()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25, Priority = 1 },
            new TestEntity { Id = 2, Name = "Alice", Age = 30, Priority = 2 },
            new TestEntity { Id = 3, Name = "Bob", Age = 20, Priority = 1 },
            new TestEntity { Id = 4, Name = "Bob", Age = 35, Priority = 3 }
        };
        var query = data.AsQueryable();
        var sorts = new[]
        {
            new QuerySort { Name = "Name", Descending = false },
            new QuerySort { Name = "Priority", Descending = true }
        };

        // Act
        var result = query.Sort(sorts).ToList();

        // Assert
        result.Should().HaveCount(4);
        result[0].Name.Should().Be("Alice");
        result[0].Priority.Should().Be(2); // Alice with higher priority first
        result[1].Name.Should().Be("Alice");
        result[1].Priority.Should().Be(1);
        result[2].Name.Should().Be("Bob");
        result[2].Priority.Should().Be(3); // Bob with higher priority first
        result[3].Name.Should().Be("Bob");
        result[3].Priority.Should().Be(1);
    }

    [Fact]
    public void Sort_WithNullQuery_ThrowsArgumentNullException()
    {
        // Arrange
        IQueryable<TestEntity> query = null!;
        var sorts = new[] { new QuerySort { Name = "Name" } };

        // Act & Assert
        var action = () => query.Sort(sorts);
        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Filter Tests

    [Fact]
    public void Filter_WithNullFilter_ReturnsOriginalQuery()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25 },
            new TestEntity { Id = 2, Name = "Bob", Age = 30 }
        };
        var query = data.AsQueryable();

        // Act
        var result = query.Filter(null);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Filter_WithValidFilter_AppliesFilter()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25 },
            new TestEntity { Id = 2, Name = "Bob", Age = 30 },
            new TestEntity { Id = 3, Name = "Charlie", Age = 25 }
        };
        var query = data.AsQueryable();
        var filter = new QueryFilter
        {
            Name = "Age",
            Value = 25,
            Operator = QueryOperators.Equal
        };

        // Act
        var result = query.Filter(filter).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.Age == 25).Should().BeTrue();
    }

    [Fact]
    public void Filter_WithGreaterThanFilter_AppliesCorrectFilter()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25 },
            new TestEntity { Id = 2, Name = "Bob", Age = 30 },
            new TestEntity { Id = 3, Name = "Charlie", Age = 35 }
        };
        var query = data.AsQueryable();
        var filter = new QueryFilter
        {
            Name = "Age",
            Value = 28,
            Operator = QueryOperators.GreaterThan
        };

        // Act
        var result = query.Filter(filter).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.Age > 28).Should().BeTrue();
    }

    [Fact]
    public void Filter_WithContainsFilter_AppliesCorrectFilter()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice Johnson", Age = 25 },
            new TestEntity { Id = 2, Name = "Bob Smith", Age = 30 },
            new TestEntity { Id = 3, Name = "Charlie Johnson", Age = 35 }
        };
        var query = data.AsQueryable();
        var filter = new QueryFilter
        {
            Name = "Name",
            Value = "Johnson",
            Operator = QueryOperators.Contains
        };

        // Act
        var result = query.Filter(filter).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.All(x => x.Name.Contains("Johnson")).Should().BeTrue();
    }

    [Fact]
    public void Filter_WithComplexFilter_AppliesCorrectLogic()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25, Status = "Active" },
            new TestEntity { Id = 2, Name = "Bob", Age = 30, Status = "Inactive" },
            new TestEntity { Id = 3, Name = "Charlie", Age = 35, Status = "Active" }
        };
        var query = data.AsQueryable();
        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters = new List<QueryFilter>
            {
                new QueryFilter { Name = "Age", Value = 30, Operator = QueryOperators.GreaterThanOrEqual },
                new QueryFilter { Name = "Status", Value = "Active", Operator = QueryOperators.Equal }
            }
        };

        // Act
        var result = query.Filter(filter).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Charlie");
    }

    [Fact]
    public void Filter_WithNullQuery_ThrowsArgumentNullException()
    {
        // Arrange
        IQueryable<TestEntity> query = null!;
        var filter = new QueryFilter { Name = "Name", Value = "Test" };

        // Act & Assert
        var action = () => query.Filter(filter);
        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region ProjectTo Tests

    [Fact]
    public void ProjectTo_WithValidMapper_ProjectsCorrectly()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25, Status = "Active" },
            new TestEntity { Id = 2, Name = "Bob", Age = 30, Status = "Inactive" }
        };
        var query = data.AsQueryable();
        var mapper = new TestMapper();

        // Act
        var result = query.ProjectTo<TestEntity, TestReadModel>(mapper).ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Alice");
        result[0].Age.Should().Be(25);
        result[0].Status.Should().Be("Active");
        result[1].Id.Should().Be(2);
        result[1].Name.Should().Be("Bob");
        result[1].Age.Should().Be(30);
        result[1].Status.Should().Be("Inactive");
    }

    [Fact]
    public void ProjectTo_WithNullQuery_ThrowsArgumentNullException()
    {
        // Arrange
        IQueryable<TestEntity> query = null!;
        var mapper = new TestMapper();

        // Act & Assert
        var action = () => query.ProjectTo<TestEntity, TestReadModel>(mapper);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ProjectTo_WithNullMapper_ThrowsArgumentNullException()
    {
        // Arrange
        var data = new[] { new TestEntity { Id = 1, Name = "Alice" } };
        var query = data.AsQueryable();

        // Act & Assert
        var action = () => query.ProjectTo<TestEntity, TestReadModel>(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void ChainedOperations_SortFilterProject_WorksTogether()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25, Status = "Active" },
            new TestEntity { Id = 2, Name = "Bob", Age = 30, Status = "Inactive" },
            new TestEntity { Id = 3, Name = "Charlie", Age = 35, Status = "Active" },
            new TestEntity { Id = 4, Name = "David", Age = 20, Status = "Active" }
        };
        var query = data.AsQueryable();
        var mapper = new TestMapper();

        var filter = new QueryFilter
        {
            Name = "Status",
            Value = "Active",
            Operator = QueryOperators.Equal
        };

        var sort = new QuerySort
        {
            Name = "Age",
            Descending = true
        };

        // Act
        var result = query
            .Filter(filter)
            .Sort(sort)
            .ProjectTo<TestEntity, TestReadModel>(mapper)
            .ToList();

        // Assert
        result.Should().HaveCount(3);
        result[0].Age.Should().Be(35); // Charlie (oldest Active)
        result[0].Name.Should().Be("Charlie");
        result[1].Age.Should().Be(25); // Alice
        result[1].Name.Should().Be("Alice");
        result[2].Age.Should().Be(20); // David (youngest Active)
        result[2].Name.Should().Be("David");
    }

    [Fact]
    public void ChainedOperations_WithComplexSortAndFilter_WorksCorrectly()
    {
        // Arrange
        var data = new[]
        {
            new TestEntity { Id = 1, Name = "Alice", Age = 25, Status = "Active", Priority = 1 },
            new TestEntity { Id = 2, Name = "Bob", Age = 30, Status = "Active", Priority = 2 },
            new TestEntity { Id = 3, Name = "Charlie", Age = 35, Status = "Inactive", Priority = 1 },
            new TestEntity { Id = 4, Name = "David", Age = 28, Status = "Active", Priority = 1 }
        };
        var query = data.AsQueryable();

        var filter = new QueryFilter
        {
            Logic = QueryLogic.And,
            Filters = new List<QueryFilter>
            {
                new QueryFilter { Name = "Status", Value = "Active", Operator = QueryOperators.Equal },
                new QueryFilter { Name = "Age", Value = 26, Operator = QueryOperators.GreaterThanOrEqual }
            }
        };

        var sorts = new[]
        {
            new QuerySort { Name = "Priority", Descending = true },
            new QuerySort { Name = "Age", Descending = false }
        };

        // Act
        var result = query
            .Filter(filter)
            .Sort(sorts)
            .ToList();

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Bob"); // Priority 2, Age 30
        result[1].Name.Should().Be("David"); // Priority 1, Age 28
    }

    #endregion
}
