using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Validation;

[RegisterSingleton<IValidator<RoleUpdateModel>>]
public partial class RoleUpdateModelValidator
    : AbstractValidator<RoleUpdateModel>
{
    public RoleUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(256);
        RuleFor(p => p.UpdatedBy).MaximumLength(100);
        #endregion
    }

}
