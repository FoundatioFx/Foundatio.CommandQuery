using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class UserQueryHandler : EntityQueryHandler<TrackerContext, Data.Entities.User, int, UserReadModel>
{
    public UserQueryHandler(TrackerContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }
}
