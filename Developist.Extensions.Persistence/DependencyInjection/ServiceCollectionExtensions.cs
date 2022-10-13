using Developist.Core.Persistence;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Linq;

namespace Developist.Extensions.Persistence.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Updates the <see cref="IUnitOfWork"/> service registration by wrapping the registered implementation in a <see cref="UnitOfWorkWrapper"/>.
        /// </summary>
        /// <remarks>
        /// Call this method after calling AddUnitOfWork. For example, <c>services.AddUnitOfWork().WrapUnitOfWork();</c>
        /// </remarks>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection WrapUnitOfWork(this IServiceCollection services)
        {
            return services.WrapUnitOfWork((uow, _) => new UnitOfWorkWrapper(uow));
        }

        /// <summary>
        /// Updates the <see cref="IUnitOfWork"/> service registration by wrapping the registered implementation in a <see cref="UnitOfWorkWrapper"/>.
        /// </summary>
        /// <remarks>
        /// Call this method after calling AddUnitOfWork. For example, <c>services.AddUnitOfWork().WrapUnitOfWork();</c>
        /// </remarks>
        /// <param name="services"></param>
        /// <param name="factory">A factory delegate to customize the <see cref="UnitOfWorkWrapper"/> that will be used to wrap the actual <see cref="IUnitOfWork"/> implementation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection WrapUnitOfWork(this IServiceCollection services, Func<IUnitOfWork, IServiceProvider, UnitOfWorkWrapper> factory)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            foreach (var oldService in services.Where(service => service.ServiceType == typeof(IUnitOfWork)).ToList())
            {
                var service = new ServiceDescriptor(
                    serviceType: typeof(ActualUnitOfWorkProvider),
                    provider => new ActualUnitOfWorkProvider(oldService, provider),
                    oldService.Lifetime);

                if (services.Remove(oldService))
                {
                    services.Add(service);

                    service = new ServiceDescriptor(
                        serviceType: typeof(IUnitOfWork),
                        provider => factory(provider.GetRequiredService<ActualUnitOfWorkProvider>().UnitOfWork, provider),
                        service.Lifetime);

                    services.Add(service);
                }
            }

            return services;
        }

        private sealed class ActualUnitOfWorkProvider
        {
            private readonly ServiceDescriptor service;
            private readonly IServiceProvider serviceProvider;

            public ActualUnitOfWorkProvider(ServiceDescriptor service, IServiceProvider serviceProvider)
            {
                this.service = service;
                this.serviceProvider = serviceProvider;
            }

            public IUnitOfWork UnitOfWork
            {
                get
                {
                    if (service.ImplementationType != null)
                    {
                        return (IUnitOfWork)ActivatorUtilities.CreateInstance(serviceProvider, service.ImplementationType);
                    }

                    if (service.ImplementationInstance != null)
                    {
                        return (IUnitOfWork)service.ImplementationInstance;
                    }

                    if (service.ImplementationFactory != null)
                    {
                        return (IUnitOfWork)service.ImplementationFactory(serviceProvider);
                    }

                    throw new InvalidOperationException($"Could not obtain an implementation type or instance for the '{typeof(IUnitOfWork)}' service.");
                }
            }
        }
    }
}
