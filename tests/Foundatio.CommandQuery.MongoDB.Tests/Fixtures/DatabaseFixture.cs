using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MongoDB.Abstracts;
using MongoDB.Driver;

using Testcontainers.MongoDb;

using XUnit.Hosting;

namespace Foundatio.CommandQuery.MongoDB.Tests.Fixtures;

public class DatabaseFixture : TestApplicationFixture, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithUsername(string.Empty)
        .WithPassword(string.Empty)
        .Build();

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }

    protected override void ConfigureApplication(HostApplicationBuilder builder)
    {
        base.ConfigureApplication(builder);

        // change database from container default
        var connectionBuilder = new MongoUrlBuilder(_mongoDbContainer.GetConnectionString())
        {
            DatabaseName = "CommandQueryTracker"
        };

        // override connection string to use docker container
        var configurationData = new Dictionary<string, string?>
        {
            ["ConnectionStrings:Tracker"] = connectionBuilder.ToString()
        };

        builder.Configuration.AddInMemoryCollection(configurationData);

        var services = builder.Services;

        services.AddHostedService<DatabaseInitializer>();
        services.AddMongoRepository("Tracker");

        services.AddCommandQuery();
        services.AddMediator();

        services.AddFoundatioCommandQueryMongoDBTests();
    }
}
