using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Queries;

namespace Foundatio.CommandQuery.Dispatcher.Tests;

public class DispatcherDataServiceTests
{
    private readonly MockDispatcher _mockDispatcher;
    private readonly DispatcherDataService _dataService;

    public DispatcherDataServiceTests()
    {
        _mockDispatcher = new MockDispatcher();
        _dataService = new DispatcherDataService(_mockDispatcher);
    }

    [Fact]
    public void Constructor_WithNullDispatcher_ThrowsArgumentNullException()
    {
        // Act & Assert
        var action = () => new DispatcherDataService(null!);
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Dispatcher_Property_ReturnsProvidedDispatcher()
    {
        // Assert
        _dataService.Dispatcher.Should().BeSameAs(_mockDispatcher);
    }

    [Fact]
    public async Task GetUser_ReturnsNull()
    {
        // Act
        var result = await _dataService.GetUser();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Get_WithValidId_CallsDispatcherWithCorrectCommand()
    {
        // Arrange
        var testModel = new TestReadModel { Id = "test-id", Name = "Test Name" };
        _mockDispatcher.SetResponse<TestReadModel>(testModel);

        // Act
        var result = await _dataService.Get<string, TestReadModel>("test-id");

        // Assert
        result.Should().Be(testModel);
        _mockDispatcher.LastRequest.Should().BeOfType<GetEntity<string, TestReadModel>>();

        var command = (GetEntity<string, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Id.Should().Be("test-id");
        command.Principal.Should().BeNull();
    }

    [Fact]
    public async Task Get_WithCacheTime_CallsDispatcherWithCommand()
    {
        // Arrange
        var testModel = new TestReadModel { Id = "test-id", Name = "Test Name" };
        var cacheTime = TimeSpan.FromMinutes(5);
        _mockDispatcher.SetResponse<TestReadModel>(testModel);

        // Act
        var result = await _dataService.Get<string, TestReadModel>("test-id", cacheTime);

        // Assert
        result.Should().Be(testModel);
        _mockDispatcher.LastRequest.Should().BeOfType<GetEntity<string, TestReadModel>>();
    }

    [Fact]
    public async Task Get_WithMultipleIds_CallsDispatcherWithCorrectCommand()
    {
        // Arrange
        var ids = new[] { "id1", "id2", "id3" };
        var testModels = new List<TestReadModel>
        {
            new() { Id = "id1", Name = "Name 1" },
            new() { Id = "id2", Name = "Name 2" },
            new() { Id = "id3", Name = "Name 3" }
        };
        _mockDispatcher.SetResponse<IReadOnlyList<TestReadModel>>(testModels);

        // Act
        var result = await _dataService.Get<string, TestReadModel>(ids);

        // Assert
        result.Should().BeEquivalentTo(testModels);
        _mockDispatcher.LastRequest.Should().BeOfType<GetEntities<string, TestReadModel>>();

        var command = (GetEntities<string, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Ids.Should().BeEquivalentTo(ids);
    }

    [Fact]
    public async Task Get_WithMultipleIds_WhenNullResponse_ReturnsEmptyList()
    {
        // Arrange
        var ids = new[] { "id1", "id2" };
        _mockDispatcher.SetResponse<IReadOnlyList<TestReadModel>>(null!);

        // Act
        var result = await _dataService.Get<string, TestReadModel>(ids);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task All_WithoutSort_CallsDispatcherWithCorrectQuery()
    {
        // Arrange
        var testModels = new List<TestReadModel>
        {
            new() { Id = "id1", Name = "Name 1" },
            new() { Id = "id2", Name = "Name 2" }
        };
        var queryResult = new QueryResult<TestReadModel> { Data = testModels };
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(queryResult);

        // Act
        var result = await _dataService.All<TestReadModel>();

        // Assert
        result.Should().BeEquivalentTo(testModels);
        _mockDispatcher.LastRequest.Should().BeOfType<QueryEntities<TestReadModel>>();

        var command = (QueryEntities<TestReadModel>)_mockDispatcher.LastRequest!;
        command.Query.Should().NotBeNull();
        command.Query.Sorts.Should().HaveCount(1);
    }

    [Fact]
    public async Task All_WithSortField_CallsDispatcherWithSortedQuery()
    {
        // Arrange
        var testModels = new List<TestReadModel>
        {
            new() { Id = "id1", Name = "Name 1" },
            new() { Id = "id2", Name = "Name 2" }
        };
        var queryResult = new QueryResult<TestReadModel> { Data = testModels };
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(queryResult);

        // Act
        var result = await _dataService.All<TestReadModel>("name");

        // Assert
        result.Should().BeEquivalentTo(testModels);

        var command = (QueryEntities<TestReadModel>)_mockDispatcher.LastRequest!;
        command.Query.Should().NotBeNull();
        command.Query.Sorts.Should().HaveCount(1);
        command.Query.Sorts[0].Name.Should().Be("name");
    }

    [Fact]
    public async Task All_WhenNullResponse_ReturnsEmptyList()
    {
        // Arrange
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(null!);

        // Act
        var result = await _dataService.All<TestReadModel>();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Query_WithQuery_CallsDispatcherWithCorrectCommand()
    {
        // Arrange
        var queryDefinition = new QueryDefinition { Page = 1, PageSize = 10 };
        var queryResult = new QueryResult<TestReadModel> { Total = 5 };
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(queryResult);

        // Act
        var result = await _dataService.Query<TestReadModel>(queryDefinition);

        // Assert
        result.Should().Be(queryResult);
        _mockDispatcher.LastRequest.Should().BeOfType<QueryEntities<TestReadModel>>();

        var command = (QueryEntities<TestReadModel>)_mockDispatcher.LastRequest!;
        command.Query.Should().Be(queryDefinition);
    }

    [Fact]
    public async Task Query_WithoutQuery_CallsDispatcherWithNullQuery()
    {
        // Arrange
        var queryResult = new QueryResult<TestReadModel> { Total = 0 };
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(queryResult);

        // Act
        var result = await _dataService.Query<TestReadModel>();

        // Assert
        result.Should().Be(queryResult);

        var command = (QueryEntities<TestReadModel>)_mockDispatcher.LastRequest!;
        command.Query.Should().BeNull();
    }

    [Fact]
    public async Task Query_WhenNullResponse_ReturnsEmptyQueryResult()
    {
        // Arrange
        _mockDispatcher.SetResponse<QueryResult<TestReadModel>>(null!);

        // Act
        var result = await _dataService.Query<TestReadModel>();

        // Assert
        result.Should().NotBeNull();
        result.Total.Should().BeNull();
        result.Data.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task Search_WithSearchText_CallsDispatcherWithSearchFilter()
    {
        // Arrange
        var searchText = "test search";
        var queryResult = new QueryResult<TestSearchModel> { Total = 1 };
        _mockDispatcher.SetResponse<QueryResult<TestSearchModel>>(queryResult);

        // Act
        var result = await _dataService.Search<TestSearchModel>(searchText);

        // Assert
        result.Should().Be(queryResult);
        _mockDispatcher.LastRequest.Should().BeOfType<QueryEntities<TestSearchModel>>();

        var command = (QueryEntities<TestSearchModel>)_mockDispatcher.LastRequest!
;
        command.Query.Should().NotBeNull();
        command.Query.Filter.Should().NotBeNull();
    }

    [Fact]
    public async Task Search_WithQueryDefinition_CombinesWithSearchFilter()
    {
        // Arrange
        var searchText = "test search";
        var existingQuery = new QueryDefinition
        {
            Filter = new QueryFilter { Name = "status", Value = "active" }
        };
        var queryResult = new QueryResult<TestSearchModel> { Total = 1 };
        _mockDispatcher.SetResponse<QueryResult<TestSearchModel>>(queryResult);

        // Act
        var result = await _dataService.Search<TestSearchModel>(searchText, existingQuery);

        // Assert
        result.Should().Be(queryResult);

        var command = (QueryEntities<TestSearchModel>)_mockDispatcher.LastRequest!;
        command.Query.Should().NotBeNull();
        command.Query.Filter.Should().NotBeNull();
    }

    [Fact]
    public async Task Save_CallsDispatcherWithUpsertCommand()
    {
        // Arrange
        var updateModel = new TestUpdateModel { Name = "Updated Name" };
        var readModel = new TestReadModel { Id = "test-id", Name = "Updated Name" };
        _mockDispatcher.SetResponse<TestReadModel>(readModel);

        // Act
        var result = await _dataService.Save<string, TestUpdateModel, TestReadModel>("test-id", updateModel);

        // Assert
        result.Should().Be(readModel);
        _mockDispatcher.LastRequest.Should().BeOfType<UpdateEntity<string, TestUpdateModel, TestReadModel>>();

        var command = (UpdateEntity<string, TestUpdateModel, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Id.Should().Be("test-id");
        command.Model.Should().Be(updateModel);
        command.Upsert.Should().BeTrue();
    }

    [Fact]
    public async Task Create_CallsDispatcherWithCreateCommand()
    {
        // Arrange
        var createModel = new TestCreateModel { Name = "New Name" };
        var readModel = new TestReadModel { Id = "new-id", Name = "New Name" };
        _mockDispatcher.SetResponse<TestReadModel>(readModel);

        // Act
        var result = await _dataService.Create<TestCreateModel, TestReadModel>(createModel);

        // Assert
        result.Should().Be(readModel);
        _mockDispatcher.LastRequest.Should().BeOfType<CreateEntity<TestCreateModel, TestReadModel>>();

        var command = (CreateEntity<TestCreateModel, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Model.Should().Be(createModel);
    }

    [Fact]
    public async Task Update_CallsDispatcherWithUpdateCommand()
    {
        // Arrange
        var updateModel = new TestUpdateModel { Name = "Updated Name" };
        var readModel = new TestReadModel { Id = "test-id", Name = "Updated Name" };
        _mockDispatcher.SetResponse<TestReadModel>(readModel);

        // Act
        var result = await _dataService.Update<string, TestUpdateModel, TestReadModel>("test-id", updateModel);

        // Assert
        result.Should().Be(readModel);
        _mockDispatcher.LastRequest.Should().BeOfType<UpdateEntity<string, TestUpdateModel, TestReadModel>>();

        var command = (UpdateEntity<string, TestUpdateModel, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Id.Should().Be("test-id");
        command.Model.Should().Be(updateModel);
        command.Upsert.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_CallsDispatcherWithDeleteCommand()
    {
        // Arrange
        var readModel = new TestReadModel { Id = "test-id", Name = "Deleted Name" };
        _mockDispatcher.SetResponse<TestReadModel>(readModel);

        // Act
        var result = await _dataService.Delete<string, TestReadModel>("test-id");

        // Assert
        result.Should().Be(readModel);
        _mockDispatcher.LastRequest.Should().BeOfType<DeleteEntity<string, TestReadModel>>();

        var command = (DeleteEntity<string, TestReadModel>)_mockDispatcher.LastRequest!;
        command.Id.Should().Be("test-id");
    }

    [Fact]
    public async Task AllMethods_UseCancellationToken()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var token = cts.Token;
        _mockDispatcher.SetResponse<TestReadModel>(new TestReadModel());

        // Act & Assert - verify cancellation token is passed through
        await _dataService.Get<string, TestReadModel>("id", cancellationToken: token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Get<string, TestReadModel>(["id"], cancellationToken: token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.All<TestReadModel>(cancellationToken: token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Query<TestReadModel>(cancellationToken: token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Create<TestCreateModel, TestReadModel>(new TestCreateModel(), token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Update<string, TestUpdateModel, TestReadModel>("id", new TestUpdateModel(), token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Save<string, TestUpdateModel, TestReadModel>("id", new TestUpdateModel(), token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);

        await _dataService.Delete<string, TestReadModel>("id", token);
        _mockDispatcher.LastCancellationToken.Should().Be(token);
    }

    // Test models
    public class TestReadModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class TestCreateModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestUpdateModel
    {
        public string Name { get; set; } = string.Empty;
    }

    public class TestSearchModel : ISupportSearch
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static IEnumerable<string> SearchFields() => ["Name"];
        public static string SortField() => "Name";
    }

    public class MockDispatcher : IDispatcher
    {
        private readonly Dictionary<Type, object?> _responses = new();

        public object? LastRequest { get; private set; }
        public CancellationToken LastCancellationToken { get; private set; }

        public void SetResponse<T>(T response)
        {
            _responses[typeof(T)] = response;
        }

        public ValueTask<TResponse?> Send<TResponse>(object request, CancellationToken cancellationToken = default)
        {
            LastRequest = request;
            LastCancellationToken = cancellationToken;

            if (_responses.TryGetValue(typeof(TResponse), out var response))
            {
                return ValueTask.FromResult((TResponse?)response);
            }

            return ValueTask.FromResult<TResponse?>(default);
        }
    }
}
