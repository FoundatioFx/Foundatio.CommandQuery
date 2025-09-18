#pragma warning disable IDE0130 // Namespace does not match folder structure

using System.Linq.Expressions;

using Foundatio.CommandQuery.Mapping;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using Entities = Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Mapping;

[RegisterSingleton<IMapper<TaskReadModel, TaskCreateModel>>]
internal sealed class TaskReadModelToTaskCreateModelMapper : MapperBase<TaskReadModel, TaskCreateModel>
{
    protected override Expression<Func<TaskReadModel, TaskCreateModel>> CreateMapping()
    {
        return source => new Models.TaskCreateModel
        {
            Id = source.Id,
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<TaskReadModel, TaskUpdateModel>>]
internal sealed class TaskReadModelToTaskUpdateModelMapper : MapperBase<TaskReadModel, TaskUpdateModel>
{
    protected override Expression<Func<TaskReadModel, TaskUpdateModel>> CreateMapping()
    {
        return source => new Models.TaskUpdateModel
        {
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
            RowVersion = source.RowVersion
        };
    }
}

[RegisterSingleton<IMapper<TaskUpdateModel, TaskCreateModel>>]
internal sealed class TaskUpdateModelToTaskCreateModelMapper : MapperBase<TaskUpdateModel, TaskCreateModel>
{
    protected override Expression<Func<TaskUpdateModel, TaskCreateModel>> CreateMapping()
    {
        return source => new Models.TaskCreateModel
        {
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskReadModel>>]
internal sealed class TaskToTaskReadModelMapper : MapperBase<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskReadModel>
{
    protected override Expression<Func<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskReadModel>> CreateMapping()
    {
        return source => new Models.TaskReadModel
        {
            Id = source.Id,
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskNameModel>>]
internal sealed class TaskToTaskNameModelMapper : MapperBase<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskNameModel>
{
    protected override Expression<Func<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskNameModel>> CreateMapping()
    {
        return source => new Models.TaskNameModel
        {
            Id = source.Id,
            Title = source.Title,
        };
    }
}

[RegisterSingleton<IMapper<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskUpdateModel>>]
internal sealed class TaskToTaskUpdateModelMapper : MapperBase<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskUpdateModel>
{
    protected override Expression<Func<Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task, TaskUpdateModel>> CreateMapping()
    {
        return source => new Models.TaskUpdateModel
        {
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

[RegisterSingleton<IMapper<TaskCreateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>>]
internal sealed class TaskCreateModelToTaskMapper : MapperBase<TaskCreateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>
{
    protected override Expression<Func<TaskCreateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>> CreateMapping()
    {
        return source => new Entities.Task
        {
            Id = source.Id,
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Created = source.Created,
            CreatedBy = source.CreatedBy,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy
        };
    }
}

[RegisterSingleton<IMapper<TaskUpdateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>>]
internal sealed class TaskUpdateModelToTaskMapper : MapperBase<TaskUpdateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>
{
    protected override Expression<Func<TaskUpdateModel, Foundatio.CommandQuery.MongoDB.Tests.Data.Entities.Task>> CreateMapping()
    {
        return source => new Entities.Task
        {
            StatusId = source.StatusId,
            PriorityId = source.PriorityId,
            Title = source.Title,
            Description = source.Description,
            StartDate = source.StartDate,
            DueDate = source.DueDate,
            CompleteDate = source.CompleteDate,
            AssignedId = source.AssignedId,
            TenantId = source.TenantId,
            IsDeleted = source.IsDeleted,
            Updated = source.Updated,
            UpdatedBy = source.UpdatedBy,
        };
    }
}

