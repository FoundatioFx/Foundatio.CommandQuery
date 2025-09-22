using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class TaskCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Task, int, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
