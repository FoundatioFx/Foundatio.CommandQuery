using Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;

using Xunit.Abstractions;

using XUnit.Hosting;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase : TestHostBase<DatabaseFixture>
{
    protected DatabaseTestBase(ITestOutputHelper output, DatabaseFixture databaseFixture)
    : base(output, databaseFixture)
    {
    }
}
