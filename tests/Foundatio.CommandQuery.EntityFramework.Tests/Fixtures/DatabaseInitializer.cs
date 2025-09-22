
using Foundatio.CommandQuery.EntityFramework.Tests.Data;
using Foundatio.CommandQuery.EntityFramework.Tests.Data.Mapping;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Task = System.Threading.Tasks.Task;

namespace Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;

public class DatabaseInitializer : IHostedService
{
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var dataContext = scope.ServiceProvider.GetRequiredService<TrackerContext>();

        // create database
        await dataContext.Database.EnsureCreatedAsync(cancellationToken);

        // seed Data
        await SeedPriority(dataContext, cancellationToken);
        await SeedStatus(dataContext, cancellationToken);
        await SeedTenant(dataContext, cancellationToken);
        await SeedRole(dataContext, cancellationToken);
        await SeedUser(dataContext, cancellationToken);
        await SeedTask(dataContext, cancellationToken);
        await SeedUserRole(dataContext, cancellationToken);
    }


    private static async Task SeedPriority(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var priorities = new[]
        {
            Constants.PriorityConstants.High,
            Constants.PriorityConstants.Normal,
            Constants.PriorityConstants.Low
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {PriorityMap.Table.Schema}.{PriorityMap.Table.Name} ON", cancellationToken);

            foreach (var priority in priorities)
            {
                if (!await dataContext.Priorities.AnyAsync(p => p.Id == priority.Id, cancellationToken))
                    await dataContext.Priorities.AddAsync(priority, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {PriorityMap.Table.Schema}.{PriorityMap.Table.Name} OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedStatus(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var statuses = new[]
        {
            Constants.StatusConstants.NotStarted,
            Constants.StatusConstants.InProgress,
            Constants.StatusConstants.Completed,
            Constants.StatusConstants.Blocked
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {StatusMap.Table.Schema}.{StatusMap.Table.Name} ON", cancellationToken);

            foreach (var status in statuses)
            {
                if (!await dataContext.Statuses.AnyAsync(s => s.Id == status.Id, cancellationToken))
                    await dataContext.Statuses.AddAsync(status, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {StatusMap.Table.Schema}.{StatusMap.Table.Name} OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedTenant(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var tenants = new[]
        {
            Constants.TenantConstants.Battlestar,
            Constants.TenantConstants.Cylons
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {TenantMap.Table.Schema}.{TenantMap.Table.Name} ON", cancellationToken);

            foreach (var tenant in tenants)
            {
                if (!await dataContext.Tenants.AnyAsync(t => t.Id == tenant.Id, cancellationToken))
                    await dataContext.Tenants.AddAsync(tenant, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {TenantMap.Table.Schema}.{TenantMap.Table.Name} OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedRole(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var roles = new[]
        {
            Constants.RoleConstants.Admiral,
            Constants.RoleConstants.Captain,
            Constants.RoleConstants.Commander,
            Constants.RoleConstants.Lieutenant,
            Constants.RoleConstants.Ensign,
            Constants.RoleConstants.Civilian,
            Constants.RoleConstants.President,
            Constants.RoleConstants.Specialist,
            Constants.RoleConstants.Cylon,
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {RoleMap.Table.Schema}.{RoleMap.Table.Name} ON", cancellationToken);

            foreach (var role in roles)
            {
                if (!await dataContext.Roles.AnyAsync(r => r.Id == role.Id, cancellationToken))
                    await dataContext.Roles.AddAsync(role, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {RoleMap.Table.Schema}.{RoleMap.Table.Name} OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedUser(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var users = new[]
        {
            Constants.UserConstants.GaiusBaltar,
            Constants.UserConstants.KaraThrace,
            Constants.UserConstants.LauraRoslin,
            Constants.UserConstants.LeeAdama,
            Constants.UserConstants.NumberSix,
            Constants.UserConstants.SaulTigh,
            Constants.UserConstants.WilliamAdama,
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {UserMap.Table.Schema}.[{UserMap.Table.Name}] ON", cancellationToken);

            foreach (var user in users)
            {
                if (!await dataContext.Users.AnyAsync(u => u.Id == user.Id, cancellationToken))
                    await dataContext.Users.AddAsync(user, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {UserMap.Table.Schema}.[{UserMap.Table.Name}] OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedTask(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var tasks = new[]
        {
            Constants.TaskConstants.DefendTheFleet,
            Constants.TaskConstants.DestroyHumans,
            Constants.TaskConstants.FindEarth,
            Constants.TaskConstants.ProtectThePresident,
        };

        var executionStrategy = dataContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await dataContext.Database.BeginTransactionAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {TaskMap.Table.Schema}.{TaskMap.Table.Name} ON", cancellationToken);

            foreach (var task in tasks)
            {
                if (!await dataContext.Tasks.AnyAsync(t => t.Id == task.Id, cancellationToken))
                    await dataContext.Tasks.AddAsync(task, cancellationToken);
            }

            await dataContext.SaveChangesAsync(cancellationToken);

            await dataContext.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {TaskMap.Table.Schema}.{TaskMap.Table.Name} OFF", cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        });
    }

    private static async Task SeedUserRole(TrackerContext dataContext, CancellationToken cancellationToken)
    {
        var userRoles = new[]
        {
            Constants.UserRoleConstants.GaiusBaltarSpecialist,
            Constants.UserRoleConstants.KaraThraceCaptain,
            Constants.UserRoleConstants.LauraRoslinPresident,
            Constants.UserRoleConstants.LeeAdamaCaptain,
            Constants.UserRoleConstants.NumberSixCylon,
            Constants.UserRoleConstants.SaulTighCommander,
            Constants.UserRoleConstants.WilliamAdamaAdmiral,
        };

        foreach (var userRole in userRoles)
        {
            if (!await dataContext.UserRoles.AnyAsync(ur => ur.UserId == userRole.UserId && ur.RoleId == userRole.RoleId, cancellationToken))
                await dataContext.UserRoles.AddAsync(userRole, cancellationToken);
        }

        await dataContext.SaveChangesAsync(cancellationToken);
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
