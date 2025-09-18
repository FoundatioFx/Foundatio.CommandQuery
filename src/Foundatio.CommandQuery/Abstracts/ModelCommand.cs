using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Foundatio.CommandQuery.Abstracts;

/// <summary>
/// Represents a base command that uses a view model to perform an operation.
/// </summary>
/// <typeparam name="TModel">The type of the model used as input for the command.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the command.</typeparam>
/// <remarks>
/// This class is typically used in a CQRS (Command Query Responsibility Segregation) pattern to define commands
/// that operate on an entity using a model and return a read model as the result.
/// </remarks>
public abstract record ModelCommand<TModel, TReadModel>
    : PrincipalCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ModelCommand{TEntityModel, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the command.</param>
    /// <param name="model">The model containing the data for the operation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="model"/> is <see langword="null"/>.</exception>
    protected ModelCommand(ClaimsPrincipal? principal, [NotNull] TModel model)
        : base(principal)
    {
        ArgumentNullException.ThrowIfNull(model);

        Model = model;
    }

    /// <summary>
    /// Gets the view model used for this command.
    /// </summary>
    /// <value>
    /// The view model containing the data for the operation.
    /// </value>
    [NotNull]
    [JsonPropertyName("model")]
    public TModel Model { get; }
}
