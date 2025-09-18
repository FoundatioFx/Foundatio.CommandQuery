using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class UserQueryHandler : EntityQueryHandler<Data.Entities.User, string, UserReadModel>
{
    public UserQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
