﻿using Microsoft.Extensions.DependencyInjection;

namespace Developist.Core.Persistence.Extensions.Tests.Fixture;

internal static class ServiceProviderHelper
{
    public static ServiceProvider ConfigureServiceProvider(Action<IServiceCollection> configureServices)
    {
        var services = new ServiceCollection();
        configureServices(services);

        return services.BuildServiceProvider();
    }
}
