namespace LXGaming.Common.Event;

public delegate Task AsyncEventHandler(object? sender, EventArgs eventArgs);

public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs eventArgs);