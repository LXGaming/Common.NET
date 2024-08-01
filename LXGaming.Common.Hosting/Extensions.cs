using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LXGaming.Common.Hosting;

public static class Extensions {

    public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly) {
        return services.AddAllServices(assembly.GetTypes());
    }

    public static IServiceCollection AddAllServicesOrdered(this IServiceCollection services, Assembly assembly) {
        return services.AddAllServices(assembly.GetTypes().OrderBy(type => type.FullName));
    }

    public static IServiceCollection AddAllServices(this IServiceCollection services, IEnumerable<Type> types) {
        foreach (var type in types) {
            if (IsValid(type) && IsService(type)) {
                services.AddServiceInternal(type);
            }
        }

        return services;
    }

    public static IServiceCollection AddAllHostedServices(this IServiceCollection services, Assembly assembly) {
        return services.AddAllHostedServices(assembly.GetTypes());
    }

    public static IServiceCollection AddAllHostedServicesOrdered(this IServiceCollection services, Assembly assembly) {
        return services.AddAllHostedServices(assembly.GetTypes().OrderBy(type => type.FullName));
    }

    public static IServiceCollection AddAllHostedServices(this IServiceCollection services, IEnumerable<Type> types) {
        foreach (var type in types) {
            if (IsValid(type) && !IsService(type) && IsHostedService(type)) {
                services.AddSingleton(typeof(IHostedService), type);
            }
        }

        return services;
    }

    public static IServiceCollection AddService<TService>(this IServiceCollection services) where TService : class {
        return services.AddService(typeof(TService));
    }

    public static IServiceCollection AddService(this IServiceCollection services, Type type) {
        if (!IsValid(type)) {
            throw new ArgumentException($"Type '{type.FullName}' is not valid.", nameof(type));
        }

        if (!IsService(type)) {
            throw new ArgumentException($"Type '{type.FullName}' is not a service.", nameof(type));
        }

        return services.AddServiceInternal(type);
    }

    private static IServiceCollection AddServiceInternal(this IServiceCollection services, Type type) {
        var attribute = type.GetCustomAttribute<ServiceAttribute>(false);
        if (attribute == null) {
            throw new ArgumentException($"Type '{type.FullName}' is missing {nameof(ServiceAttribute)}.", nameof(type));
        }

        if (IsHostedService(type)) {
            if (attribute.Lifetime == ServiceLifetime.Singleton) {
                if (attribute.Type == null || attribute.Type == typeof(IHostedService)) {
                    return services.AddHostedService(type);
                }

                return services
                    .AddHostedService(type)
                    .AddSingleton(attribute.Type, provider => provider.GetRequiredService(type));
            }

            throw new InvalidOperationException($"{nameof(IHostedService)} cannot be {attribute.Lifetime}.");
        }

        services.Add(new ServiceDescriptor(attribute.Type ?? type, type, attribute.Lifetime));
        return services;
    }

    private static IServiceCollection AddHostedService(this IServiceCollection services, Type type) {
        return services
            .AddSingleton(type, type)
            .AddSingleton(typeof(IHostedService), provider => provider.GetRequiredService(type));
    }

    private static bool IsHostedService(Type type) {
        return type.IsAssignableTo(typeof(IHostedService));
    }

    private static bool IsService(Type type) {
        return type.IsDefined(typeof(ServiceAttribute), false);
    }

    private static bool IsValid(Type type) {
        return type is { IsAbstract: false, IsInterface: false };
    }
}