using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class TaskQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.Task, int, TaskReadModel>
{
    public TaskQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
