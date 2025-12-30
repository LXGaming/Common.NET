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

#if NET10_0_OR_GREATER
    public static Task InvokeAsync<TSender, TEventArgs>(this AsyncEventHandler<TSender, TEventArgs>? eventHandler,
        TSender sender, TEventArgs eventArgs) {
        if (eventHandler == null) {
            return Task.CompletedTask;
        }

        var tasks = eventHandler.GetInvocationList()
            .Cast<AsyncEventHandler<TSender, TEventArgs>>()
            .Select(handler => handler(sender, eventArgs));
        return Task.WhenAll(tasks);
    }
#endif
}