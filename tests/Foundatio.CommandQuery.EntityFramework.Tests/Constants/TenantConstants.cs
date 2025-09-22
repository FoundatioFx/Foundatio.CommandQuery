using Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class TenantConstants
{
    public static readonly Tenant Battlestar = new()
    {
        Id = 1,
        Name = "Battlestar",
        Description = "Battlestar Tenant",
        IsActive = true
    };

    public static readonly Tenant Cylons = new()
    {
        Id = 2,
        Name = "Cylons",
        Description = "Cylons Tenant",
        IsActive = true
    };
}
