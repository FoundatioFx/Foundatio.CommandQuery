namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

public partial class RoleUpdateModel
    : EntityUpdateModel
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
