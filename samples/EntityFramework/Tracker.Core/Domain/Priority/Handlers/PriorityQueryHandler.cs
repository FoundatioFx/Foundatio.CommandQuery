using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class PriorityQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Priority, int, PriorityReadModel>
{
    public PriorityQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
