using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class TenantCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Tenant, int, TenantReadModel, TenantCreateModel, TenantUpdateModel>
{
    public TenantCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
