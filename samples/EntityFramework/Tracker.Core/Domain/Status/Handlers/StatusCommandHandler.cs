using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class StatusCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Status, int, StatusReadModel, StatusCreateModel, StatusUpdateModel>
{
    public StatusCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
