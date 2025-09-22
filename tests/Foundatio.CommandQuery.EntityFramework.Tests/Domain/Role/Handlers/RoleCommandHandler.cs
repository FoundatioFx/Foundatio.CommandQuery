using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class RoleCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.Role, int, RoleReadModel, RoleCreateModel, RoleUpdateModel>
{
    public RoleCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
