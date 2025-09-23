using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class StatusQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Status, int, StatusReadModel>
{
    public StatusQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
