using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class PriorityQueryHandler : EntityQueryHandler<Data.Entities.Priority, string, PriorityReadModel>
{
    public PriorityQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
