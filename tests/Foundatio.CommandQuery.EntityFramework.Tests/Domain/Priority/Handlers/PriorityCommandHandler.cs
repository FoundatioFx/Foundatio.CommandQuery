using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class PriorityCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Priority, int, PriorityReadModel, PriorityCreateModel, PriorityUpdateModel>
{
    public PriorityCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
