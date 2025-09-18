using Foundatio.CommandQuery.Commands;
using Foundatio.CommandQuery.MongoDB.Tests.Domain.Models;
using Foundatio.CommandQuery.MongoDB.Tests.Fixtures;
using Foundatio.CommandQuery.MongoDB.Tests.Mocks;

using MediatR.CommandQuery.MongoDB.Tests;

using Microsoft.Extensions.DependencyInjection;

namespace Foundatio.CommandQuery.MongoDB.Tests;

public class PriorityTests : DatabaseTestBase
{
    public PriorityTests(ITestOutputHelper output, DatabaseFixture databaseFixture)
        : base(output, databaseFixture)
    {
    }

    [Fact]
    public async Task CreateEntity()
    {
        var mediator = Services.GetService<IMediator>();
        mediator.Should().NotBeNull();

        var generator = new Faker<PriorityCreateModel>()
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.DisplayOrder, f => f.Random.Int(1, 100))
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        var createModel = generator.Generate();

        var createCommand = new CreateEntity<PriorityCreateModel, PriorityReadModel>(MockPrincipal.Default, createModel);
        var createResult = await mediator.InvokeAsync<Result<PriorityReadModel>>(createCommand);

        createResult.Should().NotBeNull();
        createResult.IsSuccess.Should().BeTrue();
        createResult.Value.Should().NotBeNull();

        // implicit cast test
        var created = (PriorityReadModel)createResult;

        created.Should().NotBeNull();
        created.Id.Should().NotBeNullOrEmpty();
        created.Name.Should().Be(createModel.Name);
        created.Description.Should().Be(createModel.Description);
        created.DisplayOrder.Should().Be(createModel.DisplayOrder);

    }
}
