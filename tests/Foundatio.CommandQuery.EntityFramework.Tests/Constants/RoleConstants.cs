using Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class RoleConstants
{
    public static readonly Role Admiral = new()
    {
        Id = 1,
        Name = "Admiral",
        Description = "Admiral Role",
    };

    public static readonly Role Commander = new()
    {
        Id = 2,
        Name = "Commander",
        Description = "Commander Role",
    };

    public static readonly Role Lieutenant = new()
    {
        Id = 3,
        Name = "Lieutenant",
        Description = "Lieutenant Role",
    };

    public static readonly Role Captain = new()
    {
        Id = 4,
        Name = "Captain",
        Description = "Captain Role",
    };

    public static readonly Role Ensign = new()
    {
        Id = 5,
        Name = "Ensign",
        Description = "Ensign Role",
    };

    public static readonly Role Specialist = new()
    {
        Id = 6,
        Name = "Specialist",
        Description = "Specialist Role",
    };

    public static readonly Role President = new()
    {
        Id = 7,
        Name = "President",
        Description = "President Role",
    };

    public static readonly Role Civilian = new()
    {
        Id = 8,
        Name = "Civilian",
        Description = "Civilian Role",
    };

    public static readonly Role Cylon = new()
    {
        Id = 9,
        Name = "Cylon",
        Description = "Cylon Role",
    };
}
