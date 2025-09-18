#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<TenantReadModel, TenantCreateModel>>]
internal sealed class TenantReadModelToTenantCreateModelMapper : MapperBase<TenantReadModel, TenantCreateModel>
{
    protected override Expression<Func<TenantReadModel, TenantCreateModel>> CreateMapping()
    {
        return source => new Models.TenantCreateModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<TenantReadModel, TenantUpdateModel>>]
internal sealed class TenantReadModelToTenantUpdateModelMapper : MapperBase<TenantReadModel, TenantUpdateModel>
{
    protected override Expression<Func<TenantReadModel, TenantUpdateModel>> CreateMapping()
    {
        return source => new Models.TenantUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
            RowVersion = source.RowVersion
        };
    }
}

[RegisterSingleton<IMapper<TenantUpdateModel, TenantCreateModel>>]
internal sealed class TenantUpdateModelToTenantCreateModelMapper : MapperBase<TenantUpdateModel, TenantCreateModel>
{
    protected override Expression<Func<TenantUpdateModel, TenantCreateModel>> CreateMapping()
    {
        return source => new Models.TenantCreateModel
        {
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<Tenant, TenantReadModel>>]
internal sealed class TenantToTenantReadModelMapper : MapperBase<Tenant, TenantReadModel>
{
    protected override Expression<Func<Tenant, TenantReadModel>> CreateMapping()
    {
        return source => new Models.TenantReadModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<Tenant, TenantUpdateModel>>]
internal sealed class TenantToTenantUpdateModelMapper : MapperBase<Tenant, TenantUpdateModel>
{
    protected override Expression<Func<Tenant, TenantUpdateModel>> CreateMapping()
    {
        return source => new Models.TenantUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<TenantCreateModel, Tenant>>]
internal sealed class TenantCreateModelToTenantMapper : MapperBase<TenantCreateModel, Tenant>
{
    protected override Expression<Func<TenantCreateModel, Tenant>> CreateMapping()
    {
        return source => new Entities.Tenant
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<TenantUpdateModel, Tenant>>]
internal sealed class TenantUpdateModelToTenantMapper : MapperBase<TenantUpdateModel, Tenant>
{
    protected override Expression<Func<TenantUpdateModel, Tenant>> CreateMapping()
    {
        return source => new Entities.Tenant
        {
            Name = source.Name,
            Description = source.Description,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

