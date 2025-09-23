using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class PriorityCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Priority, int, PriorityReadModel, PriorityCreateModel, PriorityUpdateModel>
{
    public PriorityCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
