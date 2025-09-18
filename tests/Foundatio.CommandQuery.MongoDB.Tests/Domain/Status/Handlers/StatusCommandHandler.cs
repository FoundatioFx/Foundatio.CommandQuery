using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

using MongoDB.Driver;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Handlers;

[RegisterSingleton]
public class StatusCommandHandler : EntityCommandHandler<Data.Entities.Status, string, StatusReadModel, StatusCreateModel, StatusUpdateModel>
{
    public StatusCommandHandler(IMongoDatabase database, IMapper mapper)
        : base(database, mapper)
    {
    }
}
