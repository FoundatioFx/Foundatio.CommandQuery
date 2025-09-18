using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class TenantQueryHandler : EntityQueryHandler<Data.Entities.Tenant, string, TenantReadModel>
{
    public TenantQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
