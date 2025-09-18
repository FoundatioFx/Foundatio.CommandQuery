#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<PriorityReadModel, PriorityCreateModel>>]
internal sealed class PriorityReadModelToPriorityCreateModelMapper
    : MapperBase<PriorityReadModel, PriorityCreateModel>
{
    protected override Expression<Func<PriorityReadModel, PriorityCreateModel>> CreateMapping()
    {
        return source => new Models.PriorityCreateModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<PriorityReadModel, PriorityUpdateModel>>]
internal sealed class PriorityReadModelToPriorityUpdateModelMapper : MapperBase<PriorityReadModel, PriorityUpdateModel>
{
    protected override Expression<Func<PriorityReadModel, PriorityUpdateModel>> CreateMapping()
    {
        return source => new Models.PriorityUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
            RowVersion = source.RowVersion
        };
    }
}

[RegisterSingleton<IMapper<PriorityUpdateModel, PriorityCreateModel>>]
internal sealed class PriorityUpdateModelToPriorityCreateModelMapper : MapperBase<PriorityUpdateModel, PriorityCreateModel>
{
    protected override Expression<Func<PriorityUpdateModel, PriorityCreateModel>> CreateMapping()
    {
        return source => new Models.PriorityCreateModel
        {
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<Priority, PriorityReadModel>>]
internal sealed class PriorityToPriorityReadModelMapper : MapperBase<Priority, PriorityReadModel>
{
    protected override Expression<Func<Priority, PriorityReadModel>> CreateMapping()
    {
        return source => new Models.PriorityReadModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<Priority, PriorityUpdateModel>>]
internal sealed class PriorityToPriorityUpdateModelMapper : MapperBase<Priority, PriorityUpdateModel>
{
    protected override Expression<Func<Priority, PriorityUpdateModel>> CreateMapping()
    {
        return source => new Models.PriorityUpdateModel
        {
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<PriorityCreateModel, Priority>>]
internal sealed class PriorityCreateModelToPriorityMapper : MapperBase<PriorityCreateModel, Priority>
{
    protected override Expression<Func<PriorityCreateModel, Priority>> CreateMapping()
    {
        return source => new Entities.Priority
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<PriorityUpdateModel, Priority>>]
internal sealed class PriorityUpdateModelToPriorityMapper : MapperBase<PriorityUpdateModel, Priority>
{
    protected override Expression<Func<PriorityUpdateModel, Priority>> CreateMapping()
    {
        return source => new Entities.Priority
        {
            Name = source.Name,
            Description = source.Description,
            DisplayOrder = source.DisplayOrder,
            IsActive = source.IsActive,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

