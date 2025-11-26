using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Validation;

[RegisterSingleton<IValidator<UserUpdateModel>>]
public partial class UserUpdateModelValidator
    : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.EmailAddress).NotEmpty();
        RuleFor(p => p.EmailAddress).MaximumLength(256);
        RuleFor(p => p.DisplayName).NotEmpty();
        RuleFor(p => p.DisplayName).MaximumLength(256);
        RuleFor(p => p.UpdatedBy).MaximumLength(100);
        #endregion
    }

}
