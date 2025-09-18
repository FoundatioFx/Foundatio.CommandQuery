using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class RoleCommandHandler : EntityCommandHandler<Data.Entities.Role, string, RoleReadModel, RoleCreateModel, RoleUpdateModel>
{
    public RoleCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
