using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Abstracts;

/// <summary>
/// Represents a base command for operations that require an identifier.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
/// <remarks>
/// This class is typically used in a CQRS (Command Query Responsibility Segregation) pattern to define commands
/// that operate on a specific entity identified by a key.
/// </remarks>
public abstract record IdentifierCommand<TKey, TResponse>
    : PrincipalCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierCommand{TKey, TResponse}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the command.</param>
    /// <param name="id">The identifier of the entity for this command.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is <see langword="null"/>.</exception>
    protected IdentifierCommand(ClaimsPrincipal? principal, [NotNull] TKey id)
        : base(principal)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    /// <summary>
    /// Gets the identifier for this command.
    /// </summary>
    /// <value>
    /// The identifier of the entity for this command.
    /// </value>
    [NotNull]
    [JsonPropertyName("id")]
    public TKey Id { get; }
}
