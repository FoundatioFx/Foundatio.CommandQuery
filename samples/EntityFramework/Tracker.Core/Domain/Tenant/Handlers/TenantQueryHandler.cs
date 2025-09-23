using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class TenantQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Tenant, int, TenantReadModel>
{
    public TenantQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
