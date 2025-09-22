namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class TaskConstants
{
    public static readonly Data.Entities.Task FindEarth = new()
    {
        Id = 1,
        Title = "Find Earth",
        Description = "Find the lost colony of Earth",
        PriorityId = PriorityConstants.Normal.Id,
        StatusId = StatusConstants.NotStarted.Id,
        AssignedId = UserConstants.LauraRoslin.Id,
        TenantId = TenantConstants.Battlestar.Id,
    };

    public static readonly Data.Entities.Task ProtectThePresident = new()
    {
        Id = 2,
        Title = "Protect the President",
        Description = "Ensure the safety of President Roslin at all costs",
        PriorityId = PriorityConstants.Normal.Id,
        StatusId = StatusConstants.NotStarted.Id,
        AssignedId = UserConstants.LeeAdama.Id,
        TenantId = TenantConstants.Battlestar.Id,
    };

    public static readonly Data.Entities.Task DefendTheFleet = new()
    {
        Id = 3,
        Title = "Defend the Fleet",
        Description = "Defend the fleet from Cylon attacks",
        PriorityId = PriorityConstants.High.Id,
        StatusId = StatusConstants.InProgress.Id,
        AssignedId = UserConstants.KaraThrace.Id,
        TenantId = TenantConstants.Battlestar.Id,
    };

    public static readonly Data.Entities.Task DestroyHumans = new()
    {
        Id = 4,
        Title = "Destroy Humans",
        Description = "Destroy all human life",
        PriorityId = PriorityConstants.Normal.Id,
        StatusId = StatusConstants.InProgress.Id,
        AssignedId = UserConstants.NumberSix.Id,
        TenantId = TenantConstants.Cylons.Id,
    };

}
