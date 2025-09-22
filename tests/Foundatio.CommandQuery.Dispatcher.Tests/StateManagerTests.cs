namespace Foundatio.CommandQuery.Dispatcher.Tests;

public class StateManagerTests
{
    [Fact]
    public void Model_WhenCreated_ShouldBeNull()
    {
        // Arrange & Act
        var manager = new StateManager<TestModel>();

        // Assert
        manager.Model.Should().BeNull();
    }

    [Fact]
    public void Set_WithValidModel_ShouldSetModel()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var model = new TestModel { Name = "Test", Value = 42 };

        // Act
        manager.Set(model);

        // Assert
        manager.Model.Should().BeSameAs(model);
        manager.Model!.Name.Should().Be("Test");
        manager.Model.Value.Should().Be(42);
    }

    [Fact]
    public void Set_WithNull_ShouldSetModelToNull()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var model = new TestModel { Name = "Test", Value = 42 };
        manager.Set(model);

        // Act
        manager.Set(null);

        // Assert
        manager.Model.Should().BeNull();
    }

    [Fact]
    public void Set_WhenCalled_ShouldTriggerOnStateChangedEvent()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var eventTriggered = false;
        EventArgs? capturedArgs = null;
        object? capturedSender = null;

        manager.OnStateChanged += (sender, args) =>
        {
            eventTriggered = true;
            capturedSender = sender;
            capturedArgs = args;
        };

        var model = new TestModel { Name = "Test" };

        // Act
        manager.Set(model);

        // Assert
        eventTriggered.Should().BeTrue();
        capturedSender.Should().BeSameAs(manager);
        capturedArgs.Should().Be(EventArgs.Empty);
    }

    [Fact]
    public void Clear_WhenCalled_ShouldSetModelToNull()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var model = new TestModel { Name = "Test", Value = 42 };
        manager.Set(model);

        // Act
        manager.Clear();

        // Assert
        manager.Model.Should().BeNull();
    }

    [Fact]
    public void Clear_WhenCalled_ShouldTriggerOnStateChangedEvent()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var model = new TestModel { Name = "Test" };
        manager.Set(model);

        var eventTriggered = false;
        manager.OnStateChanged += (sender, args) => eventTriggered = true;

        // Act
        manager.Clear();

        // Assert
        eventTriggered.Should().BeTrue();
    }

    [Fact]
    public void New_WhenCalled_ShouldCreateNewModelInstance()
    {
        // Arrange
        var manager = new StateManager<TestModel>();

        // Act
        manager.New();

        // Assert
        manager.Model.Should().NotBeNull();
        manager.Model.Should().BeOfType<TestModel>();
        manager.Model!.Name.Should().Be(string.Empty);
        manager.Model.Value.Should().Be(0);
    }

    [Fact]
    public void New_WhenCalled_ShouldTriggerOnStateChangedEvent()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var eventTriggered = false;
        manager.OnStateChanged += (sender, args) => eventTriggered = true;

        // Act
        manager.New();

        // Assert
        eventTriggered.Should().BeTrue();
    }

    [Fact]
    public void New_WhenCalledMultipleTimes_ShouldCreateDifferentInstances()
    {
        // Arrange
        var manager = new StateManager<TestModel>();

        // Act
        manager.New();
        var firstInstance = manager.Model;
        manager.New();
        var secondInstance = manager.Model;

        // Assert
        firstInstance.Should().NotBeNull();
        secondInstance.Should().NotBeNull();
        firstInstance.Should().NotBeSameAs(secondInstance);
    }

    [Fact]
    public void NotifyStateChanged_WhenCalled_ShouldTriggerOnStateChangedEvent()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var eventTriggered = false;
        EventArgs? capturedArgs = null;
        object? capturedSender = null;

        manager.OnStateChanged += (sender, args) =>
        {
            eventTriggered = true;
            capturedSender = sender;
            capturedArgs = args;
        };

        // Act
        manager.NotifyStateChanged();

        // Assert
        eventTriggered.Should().BeTrue();
        capturedSender.Should().BeSameAs(manager);
        capturedArgs.Should().Be(EventArgs.Empty);
    }

    [Fact]
    public void NotifyStateChanged_WithoutSubscribers_ShouldNotThrow()
    {
        // Arrange
        var manager = new StateManager<TestModel>();

        // Act & Assert
        var action = () => manager.NotifyStateChanged();
        action.Should().NotThrow();
    }

    [Fact]
    public void OnStateChanged_WithMultipleSubscribers_ShouldNotifyAll()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var firstEventTriggered = false;
        var secondEventTriggered = false;

        manager.OnStateChanged += (sender, args) => firstEventTriggered = true;
        manager.OnStateChanged += (sender, args) => secondEventTriggered = true;

        // Act
        manager.NotifyStateChanged();

        // Assert
        firstEventTriggered.Should().BeTrue();
        secondEventTriggered.Should().BeTrue();
    }

    [Fact]
    public void StateModelManager_OperationsSequence_ShouldWorkCorrectly()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var eventCount = 0;
        manager.OnStateChanged += (sender, args) => eventCount++;

        // Act & Assert
        // Initial state
        manager.Model.Should().BeNull();
        eventCount.Should().Be(0);

        // Create new instance
        manager.New();
        manager.Model.Should().NotBeNull();
        eventCount.Should().Be(1);

        // Set specific model
        var specificModel = new TestModel { Name = "Specific", Value = 123 };
        manager.Set(specificModel);
        manager.Model.Should().BeSameAs(specificModel);
        eventCount.Should().Be(2);

        // Clear model
        manager.Clear();
        manager.Model.Should().BeNull();
        eventCount.Should().Be(3);

        // Manual notification
        manager.NotifyStateChanged();
        manager.Model.Should().BeNull();
        eventCount.Should().Be(4);
    }

    [Fact]
    public void OnStateChanged_EventUnsubscription_ShouldWork()
    {
        // Arrange
        var manager = new StateManager<TestModel>();
        var eventTriggered = false;

        void EventHandler(object? sender, EventArgs args) => eventTriggered = true;

        manager.OnStateChanged += EventHandler;

        // Act - trigger event with subscription
        manager.NotifyStateChanged();
        var firstTrigger = eventTriggered;

        // Reset and unsubscribe
        eventTriggered = false;
        manager.OnStateChanged -= EventHandler;

        // Trigger event after unsubscription
        manager.NotifyStateChanged();
        var secondTrigger = eventTriggered;

        // Assert
        firstTrigger.Should().BeTrue();
        secondTrigger.Should().BeFalse();
    }

    [Fact]
    public void StateModelManager_WithComplexModel_ShouldWorkCorrectly()
    {
        // Arrange
        var manager = new StateManager<ComplexModel>();

        // Act
        manager.New();

        // Assert
        manager.Model.Should().NotBeNull();
        manager.Model!.Items.Should().NotBeNull();
        manager.Model.Items.Should().BeEmpty();
        manager.Model.CreatedAt.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    private class TestModel
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    private class ComplexModel
    {
        public List<string> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
