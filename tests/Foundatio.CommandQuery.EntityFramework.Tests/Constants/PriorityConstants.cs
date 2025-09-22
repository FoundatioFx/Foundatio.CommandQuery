namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class PriorityConstants
{
    ///<summary>High Priority</summary>
    public static readonly Data.Entities.Priority High = new()
    {
        Id = 1,
        Name = "High",
        Description = "High priority",
        DisplayOrder = 1,
        IsActive = true
    };

    ///<summary>Normal Priority</summary>
    public static readonly Data.Entities.Priority Normal = new()
    {
        Id = 2,
        Name = "Normal",
        Description = "Normal priority",
        DisplayOrder = 2,
        IsActive = true
    };

    ///<summary>Low Priority</summary>
    public static readonly Data.Entities.Priority Low = new()
    {
        Id = 3,
        Name = "Low",
        Description = "Low priority",
        DisplayOrder = 3,
        IsActive = true
    };
}
