using Developist.Core.Persistence;
using Developist.Core.Persistence.Extensions;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> related to the configuration and registration of persistence services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Wraps the existing implementation of the <see cref="IUnitOfWork"/> service with a <see cref="UnitOfWorkWrapper"/>.
        /// </summary>
        /// <remarks>
        /// Call this method after registering the <see cref="IUnitOfWork"/> service, such as by using the <c>AddUnitOfWork</c> method: <c>services.AddUnitOfWork().WrapUnitOfWork();</c>
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to update.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection WrapUnitOfWork(this IServiceCollection services)
        {
            return services.WrapUnitOfWork((uow, _) => new UnitOfWorkWrapper(uow));
        }

        /// <summary>
        /// Wraps the existing implementation of the <see cref="IUnitOfWork"/> service with a custom <see cref="UnitOfWorkWrapper"/>.
        /// </summary>
        /// <remarks>
        /// Call this method after registering the <see cref="IUnitOfWork"/> service, such as by using the <c>AddUnitOfWork</c> method: <c>services.AddUnitOfWork().WrapUnitOfWork();</c>
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection"/> to update.</param>
        /// <param name="factory">A factory delegate to create a custom <see cref="UnitOfWorkWrapper"/> instance.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection WrapUnitOfWork(this IServiceCollection services, Func<IUnitOfWork, IServiceProvider, UnitOfWorkWrapper> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            foreach (var oldService in services.Where(service => service.ServiceType == typeof(IUnitOfWork)).ToList())
            {
                var newService = new ServiceDescriptor(
                    serviceType: typeof(ActualUnitOfWorkProvider),
                    provider => new ActualUnitOfWorkProvider(oldService, provider),
                    oldService.Lifetime);

                if (services.Remove(oldService))
                {
                    services.Add(newService);

                    newService = new ServiceDescriptor(
                        serviceType: typeof(IUnitOfWork),
                        provider => factory(provider.GetRequiredService<ActualUnitOfWorkProvider>().UnitOfWork, provider),
                        newService.Lifetime);

                    services.Add(newService);
                }
            }

            return services;
        }

        private sealed class ActualUnitOfWorkProvider
        {
            private readonly ServiceDescriptor _service;
            private readonly IServiceProvider _serviceProvider;

            public ActualUnitOfWorkProvider(ServiceDescriptor service, IServiceProvider serviceProvider)
            {
                _service = service;
                _serviceProvider = serviceProvider;
            }

            public IUnitOfWork UnitOfWork
            {
                get
                {
                    if (_service.ImplementationType != null)
                    {
                        return (IUnitOfWork)ActivatorUtilities.CreateInstance(_serviceProvider, _service.ImplementationType);
                    }

                    if (_service.ImplementationInstance != null)
                    {
                        return (IUnitOfWork)_service.ImplementationInstance;
                    }

                    if (_service.ImplementationFactory != null)
                    {
                        return (IUnitOfWork)_service.ImplementationFactory.Invoke(_serviceProvider);
                    }

                    throw new InvalidOperationException($"Failed to obtain an implementation for the '{typeof(IUnitOfWork)}' service. Make sure the service is registered correctly.");
                }
            }
        }
    }
}
