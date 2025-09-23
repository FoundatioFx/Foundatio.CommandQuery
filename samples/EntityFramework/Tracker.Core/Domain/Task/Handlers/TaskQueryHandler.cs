using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class TaskQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Task, int, TaskReadModel>
{
    public TaskQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
