using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class TenantCommandHandler : EntityCommandHandler<Data.Entities.Tenant, string, TenantReadModel, TenantCreateModel, TenantUpdateModel>
{
    public TenantCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
