#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<StatusReadModel, StatusCreateModel>>]
internal sealed class StatusReadModelToStatusCreateModelMapper : MapperBase<StatusReadModel, StatusCreateModel>
{
    protected override Expression<Func<StatusReadModel, StatusCreateModel>> CreateMapping()
    {
        return source => new Models.StatusCreateModel
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

[RegisterSingleton<IMapper<StatusReadModel, StatusUpdateModel>>]
internal sealed class StatusReadModelToStatusUpdateModelMapper : MapperBase<StatusReadModel, StatusUpdateModel>
{
    protected override Expression<Func<StatusReadModel, StatusUpdateModel>> CreateMapping()
    {
        return source => new Models.StatusUpdateModel
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

[RegisterSingleton<IMapper<StatusUpdateModel, StatusCreateModel>>]
internal sealed class StatusUpdateModelToStatusCreateModelMapper : MapperBase<StatusUpdateModel, StatusCreateModel>
{
    protected override Expression<Func<StatusUpdateModel, StatusCreateModel>> CreateMapping()
    {
        return source => new Models.StatusCreateModel
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

[RegisterSingleton<IMapper<Status, StatusReadModel>>]
internal sealed class StatusToStatusReadModelMapper : MapperBase<Status, StatusReadModel>
{
    protected override Expression<Func<Status, StatusReadModel>> CreateMapping()
    {
        return source => new Models.StatusReadModel
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

[RegisterSingleton<IMapper<Status, StatusUpdateModel>>]
internal sealed class StatusToStatusUpdateModelMapper : MapperBase<Status, StatusUpdateModel>
{
    protected override Expression<Func<Status, StatusUpdateModel>> CreateMapping()
    {
        return source => new Models.StatusUpdateModel
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

[RegisterSingleton<IMapper<StatusCreateModel, Status>>]
internal sealed class StatusCreateModelToStatusMapper : MapperBase<StatusCreateModel, Status>
{
    protected override Expression<Func<StatusCreateModel, Status>> CreateMapping()
    {
        return source => new Entities.Status
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

[RegisterSingleton<IMapper<StatusUpdateModel, Status>>]
internal sealed class StatusUpdateModelToStatusMapper : MapperBase<StatusUpdateModel, Status>
{
    protected override Expression<Func<StatusUpdateModel, Status>> CreateMapping()
    {
        return source => new Entities.Status
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

