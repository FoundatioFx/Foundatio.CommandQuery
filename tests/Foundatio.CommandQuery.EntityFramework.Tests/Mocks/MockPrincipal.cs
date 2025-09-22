using System.Security.Claims;

using Foundatio.CommandQuery.EntityFramework.Tests.Constants;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Mocks;

public static class MockPrincipal
{
    static MockPrincipal()
    {
        Default = CreatePrincipal(
            email: "william.adama@battlestar.com",
            name: "William Adama",
            userId: UserConstants.WilliamAdama.Id.ToString(),
            tenantId: TenantConstants.Battlestar.Id.ToString());
    }

    public static ClaimsPrincipal Default { get; }

    public static ClaimsPrincipal CreatePrincipal(string email, string name, string userId, string tenantId)
    {
        var claimsIdentity = new ClaimsIdentity("Identity.Application", ClaimTypes.Name, ClaimTypes.Role);
        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, name));

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, email));
        claimsIdentity.AddClaim(new Claim("tenant_id", tenantId));

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        return claimsPrincipal;
    }
}
