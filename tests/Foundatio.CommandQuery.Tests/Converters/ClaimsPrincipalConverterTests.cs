using System.Security.Claims;
using System.Text.Json;

using AwesomeAssertions;

using Foundatio.CommandQuery.Converters;

namespace Foundatio.CommandQuery.Tests.Converters;

public class ClaimsPrincipalConverterTests
{
    private readonly JsonSerializerOptions _options;

    public ClaimsPrincipalConverterTests()
    {
        _options = new JsonSerializerOptions();
        _options.Converters.Add(new ClaimsPrincipalConverter());
    }

    [Fact]
    public void JsonSerialization_WithNullPrincipal_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        ClaimsPrincipal? principal = null;

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        json.Should().Be("null");
        deserializedPrincipal.Should().BeNull();
    }

    [Fact]
    public void JsonSerialization_WithPrincipalWithoutClaimsIdentity_SerializesToNull()
    {
        // Arrange
        var principal = new ClaimsPrincipal(); // This creates a principal without ClaimsIdentity

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        json.Should().Be("null");
        deserializedPrincipal.Should().BeNull();
    }

    [Fact]
    public void JsonSerialization_WithEmptyPrincipal_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity(); // Create an empty ClaimsIdentity
        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        deserializedPrincipal!.Identity.Should().NotBeNull();
        deserializedPrincipal.Claims.Should().BeEmpty();
    }

    [Fact]
    public void JsonSerialization_WithBasicIdentity_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        deserializedPrincipal!.Identity.Should().NotBeNull();
        deserializedPrincipal.Identity!.AuthenticationType.Should().Be("Bearer");
        deserializedPrincipal.Claims.Should().BeEmpty();
    }

    [Fact]
    public void JsonSerialization_WithCustomClaimTypes_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer", "username", "role");
        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        var deserializedIdentity = deserializedPrincipal!.Identity as ClaimsIdentity;
        deserializedIdentity.Should().NotBeNull();
        deserializedIdentity!.AuthenticationType.Should().Be("Bearer");
        deserializedIdentity.NameClaimType.Should().Be("username");
        deserializedIdentity.RoleClaimType.Should().Be("role");
    }

    [Fact]
    public void JsonSerialization_WithBasicClaims_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "john.doe"));
        identity.AddClaim(new Claim(ClaimTypes.Email, "john.doe@example.com"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        deserializedPrincipal!.Claims.Should().HaveCount(3);

        var nameClaim = deserializedPrincipal.FindFirst(ClaimTypes.Name);
        nameClaim.Should().NotBeNull();
        nameClaim!.Value.Should().Be("john.doe");

        var emailClaim = deserializedPrincipal.FindFirst(ClaimTypes.Email);
        emailClaim.Should().NotBeNull();
        emailClaim!.Value.Should().Be("john.doe@example.com");

        var roleClaim = deserializedPrincipal.FindFirst(ClaimTypes.Role);
        roleClaim.Should().NotBeNull();
        roleClaim!.Value.Should().Be("Admin");
    }

    [Fact]
    public void JsonSerialization_WithClaimsWithIssuer_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "john.doe"));
        identity.AddClaim(new Claim(ClaimTypes.Email, "john.doe@example.com"));

        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        deserializedPrincipal!.Claims.Should().HaveCount(2);

        var nameClaim = deserializedPrincipal.FindFirst(ClaimTypes.Name);
        nameClaim.Should().NotBeNull();
        nameClaim!.Value.Should().Be("john.doe");

        var emailClaim = deserializedPrincipal.FindFirst(ClaimTypes.Email);
        emailClaim.Should().NotBeNull();
        emailClaim!.Value.Should().Be("john.doe@example.com");
    }

    [Fact]
    public void JsonSerialization_WithDifferentValueTypes_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim("age", "30"));
        identity.AddClaim(new Claim("verified", "true"));
        identity.AddClaim(new Claim("created", "2024-01-15T10:30:00Z"));

        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        deserializedPrincipal!.Claims.Should().HaveCount(3);

        var ageClaim = deserializedPrincipal.FindFirst("age");
        ageClaim.Should().NotBeNull();
        ageClaim!.Value.Should().Be("30");

        var verifiedClaim = deserializedPrincipal.FindFirst("verified");
        verifiedClaim.Should().NotBeNull();
        verifiedClaim!.Value.Should().Be("true");

        var createdClaim = deserializedPrincipal.FindFirst("created");
        createdClaim.Should().NotBeNull();
        createdClaim!.Value.Should().Be("2024-01-15T10:30:00Z");
    }

    [Fact]
    public void JsonSerialization_WithComplexPrincipal_SerializesAndDeserializesCorrectly()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer", ClaimTypes.Name, ClaimTypes.Role);
        identity.AddClaim(new Claim(ClaimTypes.Name, "john.doe"));
        identity.AddClaim(new Claim(ClaimTypes.Email, "john.doe@example.com"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
        identity.AddClaim(new Claim("custom_claim", "custom_value"));

        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();

        var deserializedIdentity = deserializedPrincipal!.Identity as ClaimsIdentity;
        deserializedIdentity.Should().NotBeNull();

        deserializedIdentity.AuthenticationType.Should().Be("Bearer");
        deserializedIdentity.NameClaimType.Should().Be(ClaimTypes.Name);
        deserializedIdentity.RoleClaimType.Should().Be(ClaimTypes.Role);

        deserializedPrincipal.Claims.Should().HaveCount(5);

        deserializedPrincipal.Identity.Should().NotBeNull();
        deserializedPrincipal.Identity.Name.Should().Be("john.doe");

        deserializedPrincipal.IsInRole("Admin").Should().BeTrue();
        deserializedPrincipal.IsInRole("User").Should().BeTrue();

        var customClaim = deserializedPrincipal.FindFirst("custom_claim");
        customClaim.Should().NotBeNull();
        customClaim!.Value.Should().Be("custom_value");
    }

    [Fact]
    public void JsonDeserialization_WithUnknownProperties_IgnoresThemGracefully()
    {
        // Arrange
        var json = """
        {
            "authenticationType": "Bearer",
            "nameClaimType": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
            "roleClaimType": "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
            "unknownProperty": "should be ignored",
            "claims": [
                {
                    "type": "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    "value": "john.doe",
                    "unknownClaimProperty": "should also be ignored"
                }
            ],
            "anotherUnknownProperty": 42
        }
        """;

        // Act
        var deserializedPrincipal = JsonSerializer.Deserialize<ClaimsPrincipal>(json, _options);

        // Assert
        deserializedPrincipal.Should().NotBeNull();
        var identity = deserializedPrincipal!.Identity as ClaimsIdentity;
        identity.Should().NotBeNull();
        identity!.AuthenticationType.Should().Be("Bearer");
        identity.NameClaimType.Should().Be(ClaimTypes.Name);
        identity.RoleClaimType.Should().Be(ClaimTypes.Role);

        deserializedPrincipal.Claims.Should().HaveCount(1);
        var nameClaim = deserializedPrincipal.FindFirst(ClaimTypes.Name);
        nameClaim.Should().NotBeNull();
        nameClaim!.Value.Should().Be("john.doe");
    }

    [Fact]
    public void JsonDeserialization_WithInvalidJson_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "invalid json";

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<ClaimsPrincipal>(invalidJson, _options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void JsonDeserialization_WithInvalidTokenType_ThrowsJsonException()
    {
        // Arrange
        var invalidJson = "\"string instead of object\"";

        // Act & Assert
        var act = () => JsonSerializer.Deserialize<ClaimsPrincipal>(invalidJson, _options);
        act.Should().Throw<JsonException>()
           .WithMessage("*Expected StartObject or Null token*");
    }

    [Fact]
    public void JsonSerialization_WithDefaultClaimProperties_SkipsDefaultValues()
    {
        // Arrange
        var identity = new ClaimsIdentity("Bearer");
        identity.AddClaim(new Claim(ClaimTypes.Name, "john.doe")); // Uses default issuer and string value type

        var principal = new ClaimsPrincipal(identity);

        // Act
        var json = JsonSerializer.Serialize(principal, _options);

        // Assert
        json.Should().NotContain("issuer");
        json.Should().NotContain("valueType");
        json.Should().Contain("\"type\"");
        json.Should().Contain("\"value\"");
    }
}
