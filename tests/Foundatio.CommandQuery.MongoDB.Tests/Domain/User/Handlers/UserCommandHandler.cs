using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class UserCommandHandler : EntityCommandHandler<Data.Entities.User, string, UserReadModel, UserCreateModel, UserUpdateModel>
{
    public UserCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
