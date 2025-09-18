using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Abstracts;

/// <summary>
/// Represents a base command for operations that require a list of identifiers.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the entities.</typeparam>
/// <typeparam name="TResponse">The type of the response returned by the command.</typeparam>
/// <remarks>
/// This class is typically used in a CQRS (Command Query Responsibility Segregation) pattern to define commands
/// that operate on multiple entities identified by a collection of keys.
/// </remarks>
public abstract record IdentifiersCommand<TKey, TResponse>
    : PrincipalCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifiersCommand{TKey, TResponse}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the command.</param>
    /// <param name="ids">The collection of identifiers for this command.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ids"/> is <see langword="null"/>.</exception>
    protected IdentifiersCommand(ClaimsPrincipal? principal, [NotNull] IReadOnlyCollection<TKey> ids)
        : base(principal)
    {
        ArgumentNullException.ThrowIfNull(ids);

        Ids = ids;
    }

    /// <summary>
    /// Gets the collection of identifiers for this command.
    /// </summary>
    /// <value>
    /// The collection of identifiers for this command.
    /// </value>
    [NotNull]
    [JsonPropertyName("ids")]
    public IReadOnlyCollection<TKey> Ids { get; }
}
