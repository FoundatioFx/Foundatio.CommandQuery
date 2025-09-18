using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class RoleQueryHandler : EntityQueryHandler<Data.Entities.Role, string, RoleReadModel>
{
    public RoleQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
