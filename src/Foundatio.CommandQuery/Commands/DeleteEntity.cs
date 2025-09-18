using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Foundatio.CommandQuery.Abstracts;
using Foundatio.CommandQuery.Definitions;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// A command to delete an entity based on the specified identifier.
/// <typeparamref name="TReadModel"/> represents the result of the command.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned after the command execution.</typeparam>
/// <remarks>
/// This command is typically used in a CQRS (Command Query Responsibility Segregation) pattern to delete an entity
/// and optionally return a read model representing the deleted entity or a related result.
/// </remarks>
public record DeleteEntity<TKey, TReadModel>
    : IdentifierCommand<TKey, TReadModel>, ICacheExpire
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteEntity{TKey, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the command.</param>
    /// <param name="id">The identifier of the entity to be deleted.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> is null.</exception>
    public DeleteEntity(ClaimsPrincipal? principal, [NotNull] TKey id) : base(principal, id)
    {
    }

    /// <summary>
    /// Gets the cache tag for the entity associated with this command.
    /// </summary>
    /// <returns>The cache tag for the entity, or <see langword="null"/> if no tag is available.</returns>
    string? ICacheExpire.GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
