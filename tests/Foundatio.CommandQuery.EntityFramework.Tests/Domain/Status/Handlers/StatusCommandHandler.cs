using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class StatusCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Status, int, StatusReadModel, StatusCreateModel, StatusUpdateModel>
{
    public StatusCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
