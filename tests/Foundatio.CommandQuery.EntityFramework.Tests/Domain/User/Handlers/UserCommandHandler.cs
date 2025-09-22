using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Handlers;

[RegisterSingleton]
public class UserCommandHandler : EntityCommandHandler<TrackerContext, Data.Entities.User, int, UserReadModel, UserCreateModel, UserUpdateModel>
{
    public UserCommandHandler(TrackerContext context, IMapper mapper)
        : base(context, mapper)
    {
    }
}
