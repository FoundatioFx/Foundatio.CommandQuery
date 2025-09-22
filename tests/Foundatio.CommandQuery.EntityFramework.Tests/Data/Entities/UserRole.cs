namespace Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

public partial class UserRole
{
    public UserRole()
    {
    }

    public int UserId { get; set; }

    public int RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
