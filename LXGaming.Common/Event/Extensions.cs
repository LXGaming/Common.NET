namespace LXGaming.Common.Event;

public static class Extensions {

    public static Task InvokeAsync(this AsyncEventHandler? eventHandler, object? sender, EventArgs eventArgs) {
        if (eventHandler == null) {
            return Task.CompletedTask;
        }

        var tasks = eventHandler.GetInvocationList()
            .Cast<AsyncEventHandler>()
            .Select(handler => handler(sender, eventArgs));
        return Task.WhenAll(tasks);
    }

    public static Task InvokeAsync<TEventArgs>(this AsyncEventHandler<TEventArgs>? eventHandler, object? sender,
        TEventArgs eventArgs) {
        if (eventHandler == null) {
            return Task.CompletedTask;
        }

        var tasks = eventHandler.GetInvocationList()
            .Cast<AsyncEventHandler<TEventArgs>>()
            .Select(handler => handler(sender, eventArgs));
        return Task.WhenAll(tasks);
    }
}