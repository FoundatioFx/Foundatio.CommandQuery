using Foundatio.CommandQuery.EntityFramework.Tests.Fixtures;

namespace Foundatio.CommandQuery.EntityFramework.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase(DatabaseFixture databaseFixture) : TestHostBase<DatabaseFixture>(databaseFixture)
{
}
