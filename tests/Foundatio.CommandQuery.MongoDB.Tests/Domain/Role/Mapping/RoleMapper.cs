#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<RoleReadModel, RoleCreateModel>>]
internal sealed class RoleReadModelToRoleCreateModelMapper : MapperBase<RoleReadModel, RoleCreateModel>
{
    protected override Expression<Func<RoleReadModel, RoleCreateModel>> CreateMapping()
    {
        return source => new Models.RoleCreateModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<RoleReadModel, RoleUpdateModel>>]
internal sealed class RoleReadModelToRoleUpdateModelMapper : MapperBase<RoleReadModel, RoleUpdateModel>
{
    protected override Expression<Func<RoleReadModel, RoleUpdateModel>> CreateMapping()
    {
        return source => new Models.RoleUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
            RowVersion = source.RowVersion
        };
    }
}

[RegisterSingleton<IMapper<RoleUpdateModel, RoleCreateModel>>]
internal sealed class RoleUpdateModelToRoleCreateModelMapper : MapperBase<RoleUpdateModel, RoleCreateModel>
{
    protected override Expression<Func<RoleUpdateModel, RoleCreateModel>> CreateMapping()
    {
        return source => new Models.RoleCreateModel
        {
            Name = source.Name,
            Description = source.Description,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<Role, RoleReadModel>>]
internal sealed class RoleToRoleReadModelMapper : MapperBase<Role, RoleReadModel>
{
    protected override Expression<Func<Role, RoleReadModel>> CreateMapping()
    {
        return source => new Models.RoleReadModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<Role, RoleUpdateModel>>]
internal sealed class RoleToRoleUpdateModelMapper : MapperBase<Role, RoleUpdateModel>
{
    protected override Expression<Func<Role, RoleUpdateModel>> CreateMapping()
    {
        return source => new Models.RoleUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<RoleCreateModel, Role>>]
internal sealed class RoleCreateModelToRoleMapper : MapperBase<RoleCreateModel, Role>
{
    protected override Expression<Func<RoleCreateModel, Role>> CreateMapping()
    {
        return source => new Entities.Role
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<RoleUpdateModel, Role>>]
internal sealed class RoleUpdateModelToRoleMapper : MapperBase<RoleUpdateModel, Role>
{
    protected override Expression<Func<RoleUpdateModel, Role>> CreateMapping()
    {
        return source => new Entities.Role
        {
            Name = source.Name,
            Description = source.Description,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

