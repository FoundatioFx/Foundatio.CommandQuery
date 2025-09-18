using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class TaskQueryHandler : EntityQueryHandler<Data.Entities.Task, string, TaskReadModel>
{
    public TaskQueryHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
