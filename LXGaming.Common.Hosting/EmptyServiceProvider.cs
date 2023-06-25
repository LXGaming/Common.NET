namespace LXGaming.Common.Hosting;

public class EmptyServiceProvider : IServiceProvider {

    public object? GetService(Type serviceType) {
        return null;
    }
}