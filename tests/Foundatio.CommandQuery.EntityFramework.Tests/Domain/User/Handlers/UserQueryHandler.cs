using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class UserQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.User, int, UserReadModel>
{
    public UserQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
