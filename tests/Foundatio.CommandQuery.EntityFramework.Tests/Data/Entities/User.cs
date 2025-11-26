namespace Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

public partial class User
    : IHaveIdentifier<int>, ITrackCreated, ITrackUpdated
{
    public User()
    {
        AssignedTasks = new HashSet<Task>();
        UserRoles = new HashSet<UserRole>();
    }

    public int Id { get; set; }

    public string EmailAddress { get; set; } = null!;

    public bool IsEmailAddressConfirmed { get; set; }

    public string DisplayName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? ResetHash { get; set; }

    public string? InviteHash { get; set; }

    public int AccessFailedCount { get; set; }

    public bool LockoutEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public DateTimeOffset? LastLogin { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; }

    public string? UpdatedBy { get; set; }

    public long RowVersion { get; set; }

    public virtual ICollection<Task> AssignedTasks { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}
