using Foundatio.CommandQuery.MongoDB.Tests.Data.Entities;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using MongoDB.Abstracts;

using Task = System.Threading.Tasks.Task;

namespace Foundatio.CommandQuery.MongoDB.Tests.Fixtures;

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
        var priorityRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<Priority>>();
        await priorityRepository.UpsertAsync(Constants.PriorityConstants.High, cancellationToken);
        await priorityRepository.UpsertAsync(Constants.PriorityConstants.Normal, cancellationToken);
        await priorityRepository.UpsertAsync(Constants.PriorityConstants.Low, cancellationToken);

        var statusRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<Status>>();
        await statusRepository.UpsertAsync(Constants.StatusConstants.NotStarted, cancellationToken);
        await statusRepository.UpsertAsync(Constants.StatusConstants.InProgress, cancellationToken);
        await statusRepository.UpsertAsync(Constants.StatusConstants.Completed, cancellationToken);
        await statusRepository.UpsertAsync(Constants.StatusConstants.Blocked, cancellationToken);
        await statusRepository.UpsertAsync(Constants.StatusConstants.Deferred, cancellationToken);
        await statusRepository.UpsertAsync(Constants.StatusConstants.Done, cancellationToken);

        var tenantRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<Tenant>>();
        await tenantRepository.UpsertAsync(Constants.TenantConstants.Battlestar, cancellationToken);
        await tenantRepository.UpsertAsync(Constants.TenantConstants.Cylons, cancellationToken);

        var userRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<User>>();
        await userRepository.UpsertAsync(Constants.UserConstants.WilliamAdama, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.LauraRoslin, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.KaraThrace, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.LeeAdama, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.GaiusBaltar, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.SaulTigh, cancellationToken);
        await userRepository.UpsertAsync(Constants.UserConstants.NumberSix, cancellationToken);

        var taskRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<Data.Entities.Task>>();
        await taskRepository.UpsertAsync(Constants.TaskConstants.DefendTheFleet, cancellationToken);
        await taskRepository.UpsertAsync(Constants.TaskConstants.ProtectThePresident, cancellationToken);
        await taskRepository.UpsertAsync(Constants.TaskConstants.FindEarth, cancellationToken);
        await taskRepository.UpsertAsync(Constants.TaskConstants.DestroyHumans, cancellationToken);

        var roleRepository = _serviceProvider.GetRequiredService<IMongoEntityRepository<Role>>();
        await roleRepository.UpsertAsync(Constants.RoleConstants.Admiral, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.President, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Commander, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Ensign, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Lieutenant, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Captain, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Civilian, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Cylon, cancellationToken);
        await roleRepository.UpsertAsync(Constants.RoleConstants.Specialist, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
