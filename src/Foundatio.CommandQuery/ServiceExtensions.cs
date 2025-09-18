using Foundatio.CommandQuery.Definitions;
using Foundatio.CommandQuery.Mapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Foundatio.CommandQuery;

public static class ServiceExtensions
{
    public static IServiceCollection AddCommandQuery(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IMapper, ServiceProviderMapper>();

        return services;
    }
}
