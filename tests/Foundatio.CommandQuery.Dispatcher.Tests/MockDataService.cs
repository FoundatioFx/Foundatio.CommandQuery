using Rocks;

namespace Foundatio.CommandQuery.Dispatcher.Tests;

[RockPartial(typeof(IDispatcherDataService), BuildType.Create)]
public sealed partial class MockDataService;
