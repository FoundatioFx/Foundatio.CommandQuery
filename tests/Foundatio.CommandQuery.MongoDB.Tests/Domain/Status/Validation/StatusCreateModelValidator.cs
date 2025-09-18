using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;

namespace Foundatio.CommandQuery.MongoDB.Tests.Domain.Validation;

[RegisterSingleton<IValidator<StatusCreateModel>>]
public partial class StatusCreateModelValidator
    : AbstractValidator<StatusCreateModel>
{
    public StatusCreateModelValidator()
    {

        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(255);

    }

}
