
namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

public partial class PriorityReadModel
    : EntityReadModel, ISupportSearch
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public static IEnumerable<string> SearchFields()
        => [nameof(Name), nameof(Description)];

    public static string SortField()
        => nameof(DisplayOrder);
}
