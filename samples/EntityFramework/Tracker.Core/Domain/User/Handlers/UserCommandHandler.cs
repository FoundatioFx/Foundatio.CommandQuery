using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.EntityFramework;

using Tracker.Data;
using Tracker.Domain.Models;

namespace Tracker.Domain.Handlers;

[RegisterScoped]
public class UserCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.User, int, UserReadModel, UserCreateModel, UserUpdateModel>
{
    public UserCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
