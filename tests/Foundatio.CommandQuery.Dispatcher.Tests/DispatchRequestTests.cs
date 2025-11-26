using System.Security.Claims;
using System.Text.Json;
using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.Models;
using Foundatio.CommandQuery.Converters;

namespace Foundatio.CommandQuery.Dispatcher.Tests;

public class DispatchRequestTests
{
    private readonly JsonSerializerOptions _options;

    public DispatchRequestTests()
    {
        _options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        _options.Converters.Add(new ClaimsPrincipalConverter());
    }

    [Fact]
    public void JsonSerialization_WithCreateEntityCommand_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity("test"));
        var createModel = new EntityCreateModel<string>
        {
            Id = "test-entity-123",
            Created = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero),
            CreatedBy = "test-user",
            Updated = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero),
            UpdatedBy = "test-user"
        };
        var createCommand = new CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>(principal, createModel);
        var dispatchRequest = DispatchRequest.Create(createCommand);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);
        var deserializedRequest = JsonSerializer.Deserialize<DispatchRequest>(json, _options);

        // Assert
        deserializedRequest.Should().NotBeNull();
        deserializedRequest!.Type.Should().Be(createCommand.GetType().AssemblyQualifiedName);
        deserializedRequest.Request.Should().NotBeNull();

        var deserializedCommand = deserializedRequest.Request.Should().BeOfType<CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>>().Subject;
        deserializedCommand.Model.Should().NotBeNull();
        deserializedCommand.Model.Id.Should().Be("test-entity-123");
        deserializedCommand.Model.CreatedBy.Should().Be("test-user");
    }

    [Fact]
    public void JsonSerialization_WithUpdateEntityCommand_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "admin-user"));
        var principal = new ClaimsPrincipal(identity);

        var updateModel = new EntityUpdateModel
        {
            Updated = new DateTimeOffset(2024, 1, 15, 11, 0, 0, TimeSpan.Zero),
            UpdatedBy = "admin-user"
        };
        var updateCommand = new UpdateEntity<string, EntityUpdateModel, EntityReadModel<string>>(principal, "entity-456", updateModel, upsert: true);
        var dispatchRequest = DispatchRequest.Create(updateCommand);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);
        var deserializedRequest = JsonSerializer.Deserialize<DispatchRequest>(json, _options);

        // Assert
        deserializedRequest.Should().NotBeNull();
        deserializedRequest!.Type.Should().Be(updateCommand.GetType().AssemblyQualifiedName);

        var deserializedCommand = deserializedRequest.Request.Should().BeOfType<UpdateEntity<string, EntityUpdateModel, EntityReadModel<string>>>().Subject;
        deserializedCommand.Id.Should().Be("entity-456");
        deserializedCommand.Upsert.Should().BeTrue();
        deserializedCommand.Model.Should().NotBeNull();
        deserializedCommand.Model.UpdatedBy.Should().Be("admin-user");
    }

    [Fact]
    public void JsonSerialization_WithDeleteEntityCommand_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity("Bearer"));
        var deleteCommand = new DeleteEntity<Guid, EntityReadModel<Guid>>(principal, Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
        var dispatchRequest = DispatchRequest.Create(deleteCommand);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);
        var deserializedRequest = JsonSerializer.Deserialize<DispatchRequest>(json, _options);

        // Assert
        deserializedRequest.Should().NotBeNull();
        deserializedRequest!.Type.Should().Be(deleteCommand.GetType().AssemblyQualifiedName);

        var deserializedCommand = deserializedRequest.Request.Should().BeOfType<DeleteEntity<Guid, EntityReadModel<Guid>>>().Subject;
        deserializedCommand.Id.Should().Be(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
    }

    [Fact]
    public void JsonSerialization_WithGetEntityQuery_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity("Bearer"));
        var getQuery = new GetEntity<int, EntityReadModel<int>>(principal, 12345);
        var dispatchRequest = DispatchRequest.Create(getQuery);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);
        var deserializedRequest = JsonSerializer.Deserialize<DispatchRequest>(json, _options);

        // Assert
        deserializedRequest.Should().NotBeNull();
        deserializedRequest!.Type.Should().Be(getQuery.GetType().AssemblyQualifiedName);

        var deserializedQuery = deserializedRequest.Request.Should().BeOfType<GetEntity<int, EntityReadModel<int>>>().Subject;
        deserializedQuery.Id.Should().Be(12345);
    }

    [Fact]
    public void JsonSerialization_WithComplexCommand_PreservesAllProperties()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "john.doe"));
        identity.AddClaim(new Claim(ClaimTypes.Email, "john.doe@example.com"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        var principal = new ClaimsPrincipal(identity);

        var createModel = new EntityCreateModel<string>
        {
            Id = "complex-entity-789",
            Created = new DateTimeOffset(2024, 1, 15, 9, 0, 0, TimeSpan.Zero),
            CreatedBy = "john.doe",
            Updated = new DateTimeOffset(2024, 1, 15, 9, 15, 0, TimeSpan.Zero),
            UpdatedBy = "john.doe"
        };
        var createCommand = new CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>(principal, createModel);
        var dispatchRequest = DispatchRequest.Create(createCommand);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);
        var deserializedRequest = JsonSerializer.Deserialize<DispatchRequest>(json, _options);

        // Assert
        deserializedRequest.Should().NotBeNull();
        var deserializedCommand = deserializedRequest!.Request.Should().BeOfType<CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>>().Subject;

        // Verify all command properties are preserved
        deserializedCommand.Model.Id.Should().Be("complex-entity-789");
        deserializedCommand.Model.Created.Should().Be(new DateTimeOffset(2024, 1, 15, 9, 0, 0, TimeSpan.Zero));
        deserializedCommand.Model.CreatedBy.Should().Be("john.doe");
        deserializedCommand.Model.Updated.Should().Be(new DateTimeOffset(2024, 1, 15, 9, 15, 0, TimeSpan.Zero));
        deserializedCommand.Model.UpdatedBy.Should().Be("john.doe");
    }

    [Fact]
    public void JsonSerialization_SerializedJsonHasCorrectStructure()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity("Bearer"));
        var createModel = new EntityCreateModel<string> { Id = "structure-test" };
        var createCommand = new CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>(principal, createModel);
        var dispatchRequest = DispatchRequest.Create(createCommand);

        // Act
        var json = JsonSerializer.Serialize(dispatchRequest, _options);

        // Assert
        json.Should().Contain("\"type\":");
        json.Should().Contain("\"request\":");
        json.Should().Contain("\"model\":");
        json.Should().Contain($"\"id\":\"structure-test\"");
        json.Should().Contain("CreateEntity"); // Just check that the type name contains the command name
        json.Should().Contain("EntityCreateModel"); // And the model type
    }

    [Fact]
    public void JsonDeserialization_WithInvalidTypeString_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = """
        {
            "type": "NonExistent.Type, NonExistent.Assembly",
            "request": { "model": { "id": "test" }, "activated": "2024-01-15T10:30:00Z" }
        }
        """;

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DispatchRequest>(invalidJson, _options);
        act.Should().Throw<JsonException>()
           .WithMessage("*could not resolve type*");
    }

    [Fact]
    public void JsonDeserialization_WithMissingTypeProperty_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = """
        {
            "request": { "model": { "id": "test" } }
        }
        """;

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DispatchRequest>(invalidJson, _options);
        act.Should().Throw<JsonException>()
           .WithMessage("*expected 'type' property name*");
    }

    [Fact]
    public void JsonDeserialization_WithMissingRequestProperty_ThrowsJsonException()
    {
        // Arrange
        var typeString = typeof(CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>).AssemblyQualifiedName;
        var invalidJson = $$"""
        {
            "type": "{{typeString}}"
        }
        """;

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DispatchRequest>(invalidJson, _options);
        act.Should().Throw<JsonException>()
           .WithMessage("*expected property name token type*");
    }

    [Fact]
    public void JsonDeserialization_WithInvalidJsonStructure_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "invalid json";

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DispatchRequest>(invalidJson, _options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void JsonDeserialization_WithStringInsteadOfObject_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "\"string instead of object\"";

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<DispatchRequest>(invalidJson, _options);
        act.Should().Throw<JsonException>()
           .WithMessage("*expected start object token type*");
    }

    [Fact]
    public void JsonSerialization_WithNullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        var dispatchRequest = new DispatchRequest
        {
            Type = typeof(CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>).AssemblyQualifiedName!,
            Request = null!
        };

        // Act & Assert
        var act = () => JsonSerializer.Serialize(dispatchRequest, _options);
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("*Request property cannot be null*");
    }

    [Fact]
    public void Create_WithValidCommand_CreatesCorrectDispatchRequest()
    {
        // Arrange
        var principal = new ClaimsPrincipal(new ClaimsIdentity("Bearer"));
        var createModel = new EntityCreateModel<string> { Id = "create-test" };
        var createCommand = new CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>(principal, createModel);

        // Act
        var dispatchRequest = DispatchRequest.Create(createCommand);

        // Assert
        dispatchRequest.Should().NotBeNull();
        dispatchRequest.Type.Should().Be(typeof(CreateEntity<EntityCreateModel<string>, EntityReadModel<string>>).AssemblyQualifiedName);
        dispatchRequest.Request.Should().BeSameAs(createCommand);
    }

    [Fact]
    public void Create_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => DispatchRequest.Create(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void JsonSerialization_RoundTripPreservesAllData()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "roundtrip.user"));
        var principal = new ClaimsPrincipal(identity);

        var updateModel = new EntityUpdateModel
        {
            Updated = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero),
            UpdatedBy = "roundtrip.user"
        };
        var updateCommand = new UpdateEntity<Guid, EntityUpdateModel, EntityReadModel<Guid>>(
            principal, Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), updateModel, upsert: false);
        var dispatchRequest = DispatchRequest.Create(updateCommand);

        // Act - Multiple round trips
        var json1 = JsonSerializer.Serialize(dispatchRequest, _options);
        var roundTrip1 = JsonSerializer.Deserialize<DispatchRequest>(json1, _options);
        var json2 = JsonSerializer.Serialize(roundTrip1, _options);
        var roundTrip2 = JsonSerializer.Deserialize<DispatchRequest>(json2, _options);

        // Assert
        var finalCommand = roundTrip2!.Request.Should().BeOfType<UpdateEntity<Guid, EntityUpdateModel, EntityReadModel<Guid>>>().Subject;
        finalCommand.Id.Should().Be(Guid.Parse("550e8400-e29b-41d4-a716-446655440000"));
        finalCommand.Upsert.Should().BeFalse();
        finalCommand.Model.Updated.Should().Be(updateModel.Updated);
        finalCommand.Model.UpdatedBy.Should().Be(updateModel.UpdatedBy);
    }

    [Fact]
    public void JsonSerialization_WithDifferentGenericTypes_SerializesCorrectly()
    {
        // Test with different key types to ensure generic type handling works correctly

        // String key
        var stringDeleteCommand = new DeleteEntity<string, EntityReadModel<string>>(null, "string-key-123");
        var stringDispatchRequest = DispatchRequest.Create(stringDeleteCommand);

        // Int key
        var intDeleteCommand = new DeleteEntity<int, EntityReadModel<int>>(null, 42);
        var intDispatchRequest = DispatchRequest.Create(intDeleteCommand);

        // Guid key
        var guidDeleteCommand = new DeleteEntity<Guid, EntityReadModel<Guid>>(null, Guid.NewGuid());
        var guidDispatchRequest = DispatchRequest.Create(guidDeleteCommand);

        // Act & Assert for each type
        var stringJson = JsonSerializer.Serialize(stringDispatchRequest, _options);
        var stringDeserialized = JsonSerializer.Deserialize<DispatchRequest>(stringJson, _options);
        var stringResult = stringDeserialized!.Request.Should().BeOfType<DeleteEntity<string, EntityReadModel<string>>>().Subject;
        stringResult.Id.Should().Be("string-key-123");

        var intJson = JsonSerializer.Serialize(intDispatchRequest, _options);
        var intDeserialized = JsonSerializer.Deserialize<DispatchRequest>(intJson, _options);
        var intResult = intDeserialized!.Request.Should().BeOfType<DeleteEntity<int, EntityReadModel<int>>>().Subject;
        intResult.Id.Should().Be(42);

        var guidJson = JsonSerializer.Serialize(guidDispatchRequest, _options);
        var guidDeserialized = JsonSerializer.Deserialize<DispatchRequest>(guidJson, _options);
        var guidResult = guidDeserialized!.Request.Should().BeOfType<DeleteEntity<Guid, EntityReadModel<Guid>>>().Subject;
        guidResult.Id.Should().Be(guidDeleteCommand.Id);
    }
}
