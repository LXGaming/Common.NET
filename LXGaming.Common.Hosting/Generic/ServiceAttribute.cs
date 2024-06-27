using Microsoft.Extensions.DependencyInjection;

namespace LXGaming.Common.Hosting.Generic;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute<T>(ServiceLifetime lifetime) : ServiceAttribute(lifetime, typeof(T));