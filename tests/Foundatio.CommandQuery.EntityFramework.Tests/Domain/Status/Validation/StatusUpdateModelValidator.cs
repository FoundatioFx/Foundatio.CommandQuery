using System;
using FluentValidation;
using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Validation;

[RegisterSingleton<IValidator<StatusUpdateModel>>]
public partial class StatusUpdateModelValidator
    : AbstractValidator<StatusUpdateModel>
{
    public StatusUpdateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Name).NotEmpty();
        RuleFor(p => p.Name).MaximumLength(100);
        RuleFor(p => p.Description).MaximumLength(255);
        RuleFor(p => p.UpdatedBy).MaximumLength(100);
        #endregion
    }

}
