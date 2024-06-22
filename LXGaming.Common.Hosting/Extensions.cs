using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LXGaming.Common.Hosting;

public static class Extensions {

    public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly) {
        foreach (var type in assembly.GetTypes()) {
            if (type is { IsAbstract: false, IsInterface: false }) {
                services.AddService(type);
            }
        }

        return services;
    }

    public static IServiceCollection AddService<TService>(this IServiceCollection services) where TService : class {
        return services.AddService(typeof(TService));
    }

    public static IServiceCollection AddService(this IServiceCollection services, Type type) {
        var serviceAttribute = type.IsDefined(typeof(ServiceAttribute)) ? type.GetCustomAttribute<ServiceAttribute>() : null;
        if (type.IsAssignableTo(typeof(IHostedService))) {
            if (serviceAttribute == null) {
                return services.AddSingleton(typeof(IHostedService), type);
            }

            if (serviceAttribute.Lifetime == ServiceLifetime.Singleton) {
                if (serviceAttribute.Type == null || serviceAttribute.Type == typeof(IHostedService)) {
                    return services.AddHostedService(type);
                }

                return services
                    .AddHostedService(type)
                    .AddSingleton(serviceAttribute.Type, provider => provider.GetRequiredService(type));
            }

            throw new InvalidOperationException($"{nameof(IHostedService)} cannot be {serviceAttribute.Lifetime}.");
        }

        if (serviceAttribute != null) {
            services.Add(new ServiceDescriptor(serviceAttribute.Type ?? type, type, serviceAttribute.Lifetime));
        }

        return services;
    }

    private static IServiceCollection AddHostedService(this IServiceCollection services, Type type) {
        return services
            .AddSingleton(type, type)
            .AddSingleton(typeof(IHostedService), provider => provider.GetRequiredService(type));
    }
}