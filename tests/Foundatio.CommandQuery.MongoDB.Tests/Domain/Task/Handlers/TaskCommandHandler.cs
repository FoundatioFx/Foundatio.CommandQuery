using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class TaskCommandHandler : EntityCommandHandler<Data.Entities.Task, string, TaskReadModel, TaskCreateModel, TaskUpdateModel>
{
    public TaskCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
