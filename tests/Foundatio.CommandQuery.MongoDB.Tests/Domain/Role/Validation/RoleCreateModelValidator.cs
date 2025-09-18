using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Validation;

[RegisterSingleton<IValidator<RoleCreateModel>>]
public partial class RoleCreateModelValidator
    : AbstractValidator<RoleCreateModel>
{
    public RoleCreateModelValidator()
    {

        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(256);

    }

}
