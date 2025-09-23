using Foundatio.CommandQuery.Definitions;

namespace Tracker.Domain.Models;

[Equatable]
public partial class UserReadModel
    : EntityReadModel, ISupportSearch
{
    #region Generated Properties
    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public bool IsDeleted { get; set; }

    #endregion

    public override string ToString()
        => $"{DisplayName} <{EmailAddress}>";

    public static IEnumerable<string> SearchFields()
        => [nameof(DisplayName), nameof(EmailAddress)];

    public static string SortField()
        => nameof(DisplayName);

}
