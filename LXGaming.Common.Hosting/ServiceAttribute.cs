using Microsoft.Extensions.DependencyInjection;

namespace LXGaming.Common.Hosting {

    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute {

        public ServiceLifetime Lifetime { get; }
        public Type? Type { get; }

        public ServiceAttribute(ServiceLifetime lifetime, Type? type = null) {
            Lifetime = lifetime;
            Type = type;
        }
    }
}