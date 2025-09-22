namespace Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

public class Priority
    : IHaveIdentifier<int>, ITrackCreated, ITrackUpdated
{
    public Priority()
    {
        Tasks = new HashSet<Task>();
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; }

    public string? UpdatedBy { get; set; }

    public long RowVersion { get; set; }

    public virtual ICollection<Task> Tasks { get; set; }
}
