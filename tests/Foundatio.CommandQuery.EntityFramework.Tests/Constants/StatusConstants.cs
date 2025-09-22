using Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class StatusConstants
{
    ///<summary>Not Started</summary>
    public static readonly Status NotStarted = new()
    {
        Id = 1,
        Name = "Not Started",
        Description = "Not Started",
        DisplayOrder = 1,
        IsActive = true
    };

    ///<summary>In Progress</summary>
    public static readonly Status InProgress = new()
    {
        Id = 2,
        Name = "In Progress",
        Description = "In Progress",
        DisplayOrder = 2,
        IsActive = true
    };

    ///<summary>Completed</summary>
    public static readonly Status Completed = new()
    {
        Id = 3,
        Name = "Completed",
        Description = "Completed",
        DisplayOrder = 3,
        IsActive = true
    };

    ///<summary>Blocked</summary>
    public static readonly Status Blocked = new()
    {
        Id = 4,
        Name = "Blocked",
        Description = "Blocked",
        DisplayOrder = 4,
        IsActive = true
    };

    ///<summary>Deferred</summary>
    public static readonly Status Deferred = new()
    {
        Id = 5,
        Name = "Deferred",
        Description = "Deferred",
        DisplayOrder = 5,
        IsActive = true
    };

    ///<summary>Done</summary>
    public static readonly Status Done = new()
    {
        Id = 6,
        Name = "Done",
        Description = "Done",
        DisplayOrder = 6,
        IsActive = true
    };
}
