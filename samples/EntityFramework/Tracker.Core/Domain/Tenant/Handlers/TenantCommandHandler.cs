using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class TenantCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Tenant, int, TenantReadModel, TenantCreateModel, TenantUpdateModel>
{
    public TenantCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
