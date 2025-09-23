using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Dispatcher;

public class DispatchRequestConverter : JsonConverter<DispatchRequest>
{
    private static readonly JsonEncodedText TypeProperty = JsonEncodedText.Encode("type");
    private static readonly JsonEncodedText RequestProperty = JsonEncodedText.Encode("request");

    public override DispatchRequest? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("JsonReader expected start object token type");

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException("JsonReader expected property name token type");

        if (!reader.ValueTextEquals(TypeProperty.EncodedUtf8Bytes))
            throw new JsonException("JsonReader expected 'type' property name");

        reader.Read();
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException("JsonReader expected string token type");

        var requestType = reader.GetString();
        if (requestType == null)
            throw new JsonException("JsonReader expected non null string value");

        var type = Type.GetType(requestType);
        if (type is null)
            throw new JsonException($"JsonReader could not resolve type {requestType}");

        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException("JsonReader expected property name token type");

        if (!reader.ValueTextEquals(RequestProperty.EncodedUtf8Bytes))
            throw new JsonException("JsonReader expected 'request' property name");

        var instance = JsonSerializer.Deserialize(ref reader, type, options);
        if (instance is null)
            throw new JsonException($"JsonReader could not deserialize type {requestType}");

        reader.Read();
        if (reader.TokenType != JsonTokenType.EndObject)
            throw new JsonException("Unexpected end when reading JSON.");

        return new DispatchRequest
        {
            Type = requestType,
            Request = instance
        };
    }

    public override void Write(Utf8JsonWriter writer, DispatchRequest value, JsonSerializerOptions options)
    {

        ArgumentNullException.ThrowIfNull(value);

        if (value.Request is null)
            throw new ArgumentNullException(nameof(value), "Request property cannot be null.");

        if (string.IsNullOrEmpty(value.Type))
        {
            value.Type = value.Request.GetType().AssemblyQualifiedName
                ?? throw new ArgumentException("Type property cannot be null or empty.", nameof(value));
        }

        writer.WriteStartObject();
        writer.WriteString(TypeProperty, value.Type);

        writer.WritePropertyName(RequestProperty);
        JsonSerializer.Serialize(writer, value.Request, value.Request.GetType(), options);

        writer.WriteEndObject();
    }
}
