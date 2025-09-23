using System;
using System.Collections.Generic;

namespace Tracker.Data.Entities;

public partial class User
    : Foundatio.CommandQuery.Definitions.IHaveIdentifier<int>, Foundatio.CommandQuery.Definitions.ITrackCreated, Foundatio.CommandQuery.Definitions.ITrackUpdated
{
    public User()
    {
        #region Generated Constructor
        AssignedTasks = new HashSet<Task>();
        #endregion
    }

    #region Generated Properties
    public int Id { get; set; }

    public string DisplayName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTimeOffset Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset Updated { get; set; }

    public string? UpdatedBy { get; set; }

    public long RowVersion { get; set; }

    #endregion

    #region Generated Relationships
    public virtual ICollection<Task> AssignedTasks { get; set; }

    #endregion

}
