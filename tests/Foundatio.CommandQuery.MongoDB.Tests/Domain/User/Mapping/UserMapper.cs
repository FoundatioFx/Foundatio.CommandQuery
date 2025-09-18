#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<UserReadModel, UserCreateModel>>]
internal sealed class UserReadModelToUserCreateModelMapper : MapperBase<UserReadModel, UserCreateModel>
{
    protected override Expression<Func<UserReadModel, UserCreateModel>> CreateMapping()
    {
        return source => new Models.UserCreateModel
        {
            Id = source.Id,
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<UserReadModel, UserUpdateModel>>]
internal sealed class UserReadModelToUserUpdateModelMapper : MapperBase<UserReadModel, UserUpdateModel>
{
    protected override Expression<Func<UserReadModel, UserUpdateModel>> CreateMapping()
    {
        return source => new Models.UserUpdateModel
        {
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
            RowVersion = source.RowVersion
        };
    }
}

[RegisterSingleton<IMapper<UserUpdateModel, UserCreateModel>>]
internal sealed class UserUpdateModelToUserCreateModelMapper : MapperBase<UserUpdateModel, UserCreateModel>
{
    protected override Expression<Func<UserUpdateModel, UserCreateModel>> CreateMapping()
    {
        return source => new Models.UserCreateModel
        {
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<User, UserReadModel>>]
internal sealed class UserToUserReadModelMapper : MapperBase<User, UserReadModel>
{
    protected override Expression<Func<User, UserReadModel>> CreateMapping()
    {
        return source => new Models.UserReadModel
        {
            Id = source.Id,
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<User, UserUpdateModel>>]
internal sealed class UserToUserUpdateModelMapper : MapperBase<User, UserUpdateModel>
{
    protected override Expression<Func<User, UserUpdateModel>> CreateMapping()
    {
        return source => new Models.UserUpdateModel
        {
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<UserCreateModel, User>>]
internal sealed class UserCreateModelToUserMapper : MapperBase<UserCreateModel, User>
{
    protected override Expression<Func<UserCreateModel, User>> CreateMapping()
    {
        return source => new Entities.User
        {
            Id = source.Id,
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<UserUpdateModel, User>>]
internal sealed class UserUpdateModelToUserMapper : MapperBase<UserUpdateModel, User>
{
    protected override Expression<Func<UserUpdateModel, User>> CreateMapping()
    {
        return source => new Entities.User
        {
            EmailAddress = source.EmailAddress,
            IsEmailAddressConfirmed = source.IsEmailAddressConfirmed,
            DisplayName = source.DisplayName,
            PasswordHash = source.PasswordHash,
            ResetHash = source.ResetHash,
            InviteHash = source.InviteHash,
            AccessFailedCount = source.AccessFailedCount,
            LockoutEnabled = source.LockoutEnabled,
            LockoutEnd = source.LockoutEnd,
            LastLogin = source.LastLogin,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

