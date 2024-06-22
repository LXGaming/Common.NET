using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LXGaming.Common.Hosting;

public static class Extensions {

    public static IServiceCollection AddAllServices(this IServiceCollection services, Assembly assembly) {
        foreach (var type in assembly.GetTypes()) {
            if (IsValid(type)) {
                services.AddServiceInternal(type);
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

        return services.AddServiceInternal(type);
    }

    private static IServiceCollection AddServiceInternal(this IServiceCollection services, Type type) {
        var attribute = type.IsDefined(typeof(ServiceAttribute)) ? type.GetCustomAttribute<ServiceAttribute>() : null;
        if (type.IsAssignableTo(typeof(IHostedService))) {
            if (attribute == null) {
                return services.AddSingleton(typeof(IHostedService), type);
            }

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

        if (attribute != null) {
            services.Add(new ServiceDescriptor(attribute.Type ?? type, type, attribute.Lifetime));
        }

        return services;
    }

    private static IServiceCollection AddHostedService(this IServiceCollection services, Type type) {
        return services
            .AddSingleton(type, type)
            .AddSingleton(typeof(IHostedService), provider => provider.GetRequiredService(type));
    }

    private static bool IsValid(Type type) {
        return type is { IsAbstract: false, IsInterface: false };
    }
}