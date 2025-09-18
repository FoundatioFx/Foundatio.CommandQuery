using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using Foundatio.CommandQuery.Abstracts;
using Foundatio.CommandQuery.Definitions;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// Represents a command to create a new entity using the specified <typeparamref name="TCreateModel"/>.
/// The result of the command will be of type <typeparamref name="TReadModel"/>.
/// </summary>
/// <typeparam name="TCreateModel">The type of the create model used to provide data for the new entity.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the command.</typeparam>
/// <remarks>
/// This command is typically used in a CQRS (Command Query Responsibility Segregation) pattern to create a new entity
/// and return a read model representing the created entity or a related result.
/// </remarks>
public record CreateEntity<TCreateModel, TReadModel>
    : ModelCommand<TCreateModel, TReadModel>, ICacheExpire
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateEntity{TCreateModel, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user for whom this command is executed.</param>
    /// <param name="model">The create model containing the data for the new entity.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    public CreateEntity(ClaimsPrincipal? principal, [NotNull] TCreateModel model)
        : base(principal, model)
    {
    }

    /// <summary>
    /// Gets the cache tag associated with the <typeparamref name="TReadModel"/>.
    /// </summary>
    /// <returns>The cache tag for the <typeparamref name="TReadModel"/>, or <see langword="null"/> if no tag is available.</returns>
    string? ICacheExpire.GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
