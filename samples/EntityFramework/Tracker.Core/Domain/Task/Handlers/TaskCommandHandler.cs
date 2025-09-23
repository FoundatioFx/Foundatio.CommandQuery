using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class TaskCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Task, int, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
