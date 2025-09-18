using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Foundatio.CommandQuery.Abstracts;
using Foundatio.CommandQuery.Definitions;

namespace Foundatio.CommandQuery.Commands;

/// <summary>
/// Represents a command to update an entity identified by a specific key using the provided update model.
/// The result of the command will be of type <typeparamref name="TReadModel"/>.
/// </summary>
/// <typeparam name="TKey">The type of the key used to identify the entity.</typeparam>
/// <typeparam name="TUpdateModel">The type of the update model containing the data for the update.</typeparam>
/// <typeparam name="TReadModel">The type of the read model returned as the result of the command.</typeparam>
/// <remarks>
/// This command is typically used in a CQRS (Command Query Responsibility Segregation) pattern to update an entity
/// and return a read model representing the updated entity or a related result.
/// </remarks>
public record UpdateEntity<TKey, TUpdateModel, TReadModel>
    : ModelCommand<TUpdateModel, TReadModel>, ICacheExpire
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateEntity{TKey, TUpdateModel, TReadModel}"/> class.
    /// </summary>
    /// <param name="principal">The <see cref="ClaimsPrincipal"/> representing the user executing the command.</param>
    /// <param name="id">The identifier of the entity to update.</param>
    /// <param name="model">The update model containing the data for the update.</param>
    /// <param name="upsert">A value indicating whether to insert the entity if it does not exist.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="id"/> or <paramref name="model"/> is <see langword="null"/>.</exception>
    public UpdateEntity(ClaimsPrincipal? principal, [NotNull] TKey id, TUpdateModel model, bool upsert = false) : base(principal, model)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
        Upsert = upsert;
    }

    /// <summary>
    /// Gets the identifier of the entity to update.
    /// </summary>
    /// <value>
    /// The identifier of the entity to update.
    /// </value>
    [NotNull]
    [JsonPropertyName("id")]
    public TKey Id { get; }

    /// <summary>
    /// Gets a value indicating whether to insert the entity if it does not exist.
    /// </summary>
    /// <value>
    /// <see langword="true"/> if the entity should be inserted if it does not exist; otherwise, <see langword="false"/>.
    /// </value>
    public bool Upsert { get; }

    /// <summary>
    /// Gets the cache tag associated with the <typeparamref name="TReadModel"/>.
    /// </summary>
    /// <returns>The cache tag for the <typeparamref name="TReadModel"/>, or <see langword="null"/> if no tag is available.</returns>
    string? ICacheExpire.GetCacheTag()
        => CacheTagger.GetTag<TReadModel>();
}
