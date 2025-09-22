using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class TenantQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Tenant, int, TenantReadModel>
{
    public TenantQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
