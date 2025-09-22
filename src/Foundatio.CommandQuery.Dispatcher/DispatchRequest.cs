using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Dispatcher;

/// <summary>
/// Represents a request to be dispatched, including its type and payload.
/// </summary>
[JsonConverter(typeof(DispatchRequestConverter))]
public class DispatchRequest
{
    /// <summary>
    /// Gets or sets the assembly-qualified type name of the request payload.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// Gets or sets the request payload to be dispatched.
    /// </summary>
    [JsonPropertyName("request")]
    public object Request { get; set; } = null!;

    /// <summary>
    /// Creates a <see cref="DispatchRequest"/> for the specified request object.
    /// </summary>
    /// <param name="request">The request object to dispatch.</param>
    /// <returns>A new <see cref="DispatchRequest"/> containing the request and its type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="request"/> is null.</exception>
    public static DispatchRequest Create(object request)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new()
        {
            Type = request.GetType().AssemblyQualifiedName!,
            Request = request
        };
    }
}
