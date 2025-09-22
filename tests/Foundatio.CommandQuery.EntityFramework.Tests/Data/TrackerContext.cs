using Microsoft.EntityFrameworkCore;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Data;

public class TrackerContext : DbContext
{
    public TrackerContext(DbContextOptions<TrackerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Entities.Priority> Priorities { get; set; } = null!;

    public virtual DbSet<Entities.Role> Roles { get; set; } = null!;

    public virtual DbSet<Entities.Status> Statuses { get; set; } = null!;

    public virtual DbSet<Entities.Task> Tasks { get; set; } = null!;

    public virtual DbSet<Entities.Tenant> Tenants { get; set; } = null!;

    public virtual DbSet<Entities.UserRole> UserRoles { get; set; } = null!;

    public virtual DbSet<Entities.User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Mapping.PriorityMap());
        modelBuilder.ApplyConfiguration(new Mapping.RoleMap());
        modelBuilder.ApplyConfiguration(new Mapping.StatusMap());
        modelBuilder.ApplyConfiguration(new Mapping.TaskMap());
        modelBuilder.ApplyConfiguration(new Mapping.TenantMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserMap());
        modelBuilder.ApplyConfiguration(new Mapping.UserRoleMap());
    }
}
