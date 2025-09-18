using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Validation;

[RegisterSingleton<IValidator<TenantCreateModel>>]
public partial class TenantCreateModelValidator
    : AbstractValidator<TenantCreateModel>
{
    public TenantCreateModelValidator()
    {

        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(255);

    }

}
