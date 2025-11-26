using Foundatio.CommandQuery.EntityFramework.Tests.Domain.Models;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Domain.Validation;

[RegisterSingleton<IValidator<TaskCreateModel>>]
public partial class TaskCreateModelValidator
    : AbstractValidator<TaskCreateModel>
{
    public TaskCreateModelValidator()
    {
        #region Generated Constructor
        RuleFor(p => p.Title).NotEmpty();
        RuleFor(p => p.Title).MaximumLength(255);
        RuleFor(p => p.CreatedBy).MaximumLength(100);
        RuleFor(p => p.UpdatedBy).MaximumLength(100);
        #endregion
    }

}
