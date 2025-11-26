using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;

namespace Foundatio.CommandQuery.MongoDB.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase(DatabaseFixture databaseFixture) : TestHostBase<DatabaseFixture>(databaseFixture)
{
}
