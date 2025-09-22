using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;

using XUnit.Hosting;

namespace Foundatio.CommandQuery.MongoDB.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase : TestHostBase<DatabaseFixture>
{
    protected DatabaseTestBase(ITestOutputHelper output, DatabaseFixture databaseFixture)
    : base(output, databaseFixture)
    {
    }
}
