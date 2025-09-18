using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class PriorityCommandHandler : EntityCommandHandler<Data.Entities.Priority, string, PriorityReadModel, PriorityCreateModel, PriorityUpdateModel>
{
    public PriorityCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
