using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LXGaming.Common.Hosting {

    public static class Extensions {

        public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                services.AddService(type);
            }

            return services;
        }

        public static IServiceCollection AddService<TService>(this IServiceCollection services) where TService : class {
            return services.AddService(typeof(TService));
        }

        public static IServiceCollection AddService(this IServiceCollection services, Type type) {
            if (!type.IsDefined(typeof(ServiceAttribute))) {
                return services;
            }

            var serviceAttribute = type.GetCustomAttribute<ServiceAttribute>();
            if (typeof(IHostedService).IsAssignableFrom(type)) {
                if (serviceAttribute == null) {
                    return services.AddSingleton(typeof(IHostedService), type);
                }

                if (serviceAttribute.Lifetime == ServiceLifetime.Singleton) {
                    return services.AddHostedService(serviceAttribute.Type ?? type, type);
                }

                throw new InvalidOperationException($"{nameof(IHostedService)} cannot be {serviceAttribute.Lifetime}");
            }

            if (serviceAttribute != null) {
                services.Add(new ServiceDescriptor(serviceAttribute.Type ?? type, type, serviceAttribute.Lifetime));
            }

            return services;
        }

        private static IServiceCollection AddHostedService(this IServiceCollection services, Type serviceType, Type implementationType) {
            return services
                .AddSingleton(serviceType, implementationType)
                .AddSingleton(provider => (IHostedService) provider.GetRequiredService(serviceType));
        }
    }
}