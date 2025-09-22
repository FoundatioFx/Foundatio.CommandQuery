using Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class UserRoleConstants
{
    public static readonly UserRole WilliamAdamaAdmiral = new()
    {
        UserId = UserConstants.WilliamAdama.Id,
        RoleId = RoleConstants.Admiral.Id
    };

    public static readonly UserRole LauraRoslinPresident = new()
    {
        UserId = UserConstants.LauraRoslin.Id,
        RoleId = RoleConstants.President.Id
    };

    public static readonly UserRole KaraThraceCaptain = new()
    {
        UserId = UserConstants.KaraThrace.Id,
        RoleId = RoleConstants.Captain.Id
    };

    public static readonly UserRole LeeAdamaCaptain = new()
    {
        UserId = UserConstants.LeeAdama.Id,
        RoleId = RoleConstants.Captain.Id
    };

    public static readonly UserRole GaiusBaltarSpecialist = new()
    {
        UserId = UserConstants.GaiusBaltar.Id,
        RoleId = RoleConstants.Specialist.Id
    };

    public static readonly UserRole SaulTighCommander = new()
    {
        UserId = UserConstants.SaulTigh.Id,
        RoleId = RoleConstants.Commander.Id
    };

    public static readonly UserRole NumberSixCylon = new()
    {
        UserId = UserConstants.NumberSix.Id,
        RoleId = RoleConstants.Cylon.Id
    };
}
