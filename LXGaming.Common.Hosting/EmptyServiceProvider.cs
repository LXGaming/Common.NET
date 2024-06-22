namespace LXGaming.Common.Hosting;

public sealed class EmptyServiceProvider : IServiceProvider {

    public static EmptyServiceProvider Instance { get; } = new();

    public object? GetService(Type serviceType) {
        return null;
    }
}