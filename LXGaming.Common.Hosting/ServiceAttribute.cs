﻿using Microsoft.Extensions.DependencyInjection;

namespace LXGaming.Common.Hosting;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ServiceAttribute(ServiceLifetime lifetime, Type? type = null) : Attribute {

    public ServiceLifetime Lifetime { get; } = lifetime;
    public Type? Type { get; } = type;
}