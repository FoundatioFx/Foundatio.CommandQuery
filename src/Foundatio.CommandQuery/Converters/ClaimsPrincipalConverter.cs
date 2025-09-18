using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Converters;

/// <summary>
/// <see cref="JsonConverter{T}"/> for <see cref="ClaimsPrincipal"/>
/// </summary>
public class ClaimsPrincipalConverter : JsonConverter<ClaimsPrincipal>
{
    private static readonly JsonEncodedText AuthenticationType = JsonEncodedText.Encode("authenticationType");
    private static readonly JsonEncodedText NameClaimType = JsonEncodedText.Encode("nameClaimType");
    private static readonly JsonEncodedText RoleClaimType = JsonEncodedText.Encode("roleClaimType");
    private static readonly JsonEncodedText Claims = JsonEncodedText.Encode("claims");
    private static readonly JsonEncodedText ClaimType = JsonEncodedText.Encode("type");
    private static readonly JsonEncodedText ClaimValue = JsonEncodedText.Encode("value");

    /// <inheritdoc />
    public override ClaimsPrincipal? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected StartObject or Null token, but found {reader.TokenType}.");

        string? authenticationType = null;
        string? nameClaimType = null;
        string? roleClaimType = null;
        List<Claim>? claims = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Expected PropertyName token, but found {reader.TokenType}.");

            if (TryReadStringProperty(ref reader, AuthenticationType, out var propertyValue))
            {
                authenticationType = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, NameClaimType, out propertyValue))
            {
                nameClaimType = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, RoleClaimType, out propertyValue))
            {
                roleClaimType = propertyValue;
            }
            else if (TryReadClaims(ref reader, out var claimsValue))
            {
                claims = claimsValue;
            }
            else
            {
                // Skip unknown properties for forward compatibility
                reader.Read();
                reader.Skip();
            }
        }

        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException("Unexpected end when reading JSON.");

        var identity = new ClaimsIdentity(authenticationType, nameClaimType, roleClaimType);

        if (claims?.Count > 0)
        {
            foreach (var claim in claims)
                identity.AddClaim(claim);
        }

        return new ClaimsPrincipal(identity);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, ClaimsPrincipal value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        if (value?.Identity is not ClaimsIdentity identity)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        if (!string.IsNullOrEmpty(identity.AuthenticationType))
            writer.WriteString(AuthenticationType, identity.AuthenticationType);

        if (!string.IsNullOrEmpty(identity.NameClaimType))
            writer.WriteString(NameClaimType, identity.NameClaimType);

        if (!string.IsNullOrEmpty(identity.RoleClaimType))
            writer.WriteString(RoleClaimType, identity.RoleClaimType);

        if (value.Claims is not null && value.Claims.Any())
        {
            writer.WritePropertyName(Claims);
            writer.WriteStartArray();

            foreach (var claim in value.Claims)
            {
                writer.WriteStartObject();
                writer.WriteString(ClaimType, claim.Type);
                writer.WriteString(ClaimValue, claim.Value);
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }

    private static bool TryReadClaims(ref Utf8JsonReader reader, out List<Claim>? claims)
    {
        if (!reader.ValueTextEquals(Claims.EncodedUtf8Bytes))
        {
            claims = default;
            return false;
        }

        reader.Read();

        if (reader.TokenType == JsonTokenType.Null)
        {
            claims = null;
            return true;
        }

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException($"Expected StartArray or Null token for claims, but found {reader.TokenType}.");

        claims = [];

        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (TryReadClaim(ref reader, out var claim) && claim != null)
                claims.Add(claim);
        }

        return true;
    }

    private static bool TryReadClaim(ref Utf8JsonReader reader, out Claim? claim)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected StartObject token for claim, but found {reader.TokenType}.");

        string? claimType = null;
        string? claimValue = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Expected PropertyName token, but found {reader.TokenType}.");

            if (TryReadStringProperty(ref reader, ClaimType, out var propertyValue))
            {
                claimType = propertyValue;
            }
            else if (TryReadStringProperty(ref reader, ClaimValue, out propertyValue))
            {
                claimValue = propertyValue;
            }
            else
            {
                // Skip unknown properties for forward compatibility
                reader.Read();
                reader.Skip();
            }
        }

        if (string.IsNullOrEmpty(claimType) || string.IsNullOrEmpty(claimValue))
        {
            claim = null;
            return false;
        }

        claim = new Claim(claimType, claimValue);

        return true;
    }

    private static bool TryReadStringProperty(ref Utf8JsonReader reader, JsonEncodedText propertyName, out string? value)
    {
        if (!reader.ValueTextEquals(propertyName.EncodedUtf8Bytes))
        {
            value = default;
            return false;
        }

        reader.Read();

        if (reader.TokenType == JsonTokenType.String)
        {
            value = reader.GetString();
            return true;
        }

        if (reader.TokenType == JsonTokenType.Null)
        {
            value = null;
            return true;
        }

        throw new JsonException($"Expected String or Null token for {propertyName}, but found {reader.TokenType}.");
    }
}
