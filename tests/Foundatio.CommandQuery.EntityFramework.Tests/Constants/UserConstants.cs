using Foundatio.CommandQuery.EntityFramework.Tests.Data.Entities;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Constants;

public static class UserConstants
{
    ///<summary>William Adama</summary>
    public static readonly User WilliamAdama = new()
    {
        Id = 1,
        DisplayName = "William Adama",
        EmailAddress = "william.adama@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Laura Roslin</summary>
    public static readonly User LauraRoslin = new()
    {
        Id = 2,
        DisplayName = "Laura Roslin",
        EmailAddress = "laura.roslin@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Kara Thrace</summary>
    public static readonly User KaraThrace = new()
    {
        Id = 3,
        DisplayName = "Kara Thrace",
        EmailAddress = "kara.thrace@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Lee Adama</summary>
    public static readonly User LeeAdama = new()
    {
        Id = 4,
        DisplayName = "Lee Adama",
        EmailAddress = "lee.adama@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Gaius Baltar</summary>
    public static readonly User GaiusBaltar = new()
    {
        Id = 5,
        DisplayName = "Gaius Baltar",
        EmailAddress = "gaius.baltar@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Saul Tigh</summary>
    public static readonly User SaulTigh = new()
    {
        Id = 6,
        DisplayName = "Saul Tigh",
        EmailAddress = "saul.tigh@galactica.com",
        IsEmailAddressConfirmed = true
    };

    ///<summary>Number Six</summary>
    public static readonly User NumberSix = new()
    {
        Id = 7,
        DisplayName = "Number Six",
        EmailAddress = "six@cylon.com",
        IsEmailAddressConfirmed = true
    };
}
