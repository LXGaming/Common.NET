namespace LXGaming.Common.Event;

public delegate Task AsyncEventHandler(object? sender, EventArgs eventArgs);

public delegate Task AsyncEventHandler<in TEventArgs>(object? sender, TEventArgs eventArgs)
#if NET10_0_OR_GREATER
    where TEventArgs : allows ref struct
#endif
    ;

#if NET10_0_OR_GREATER
public delegate Task AsyncEventHandler<in TSender, in TEventArgs>(TSender sender, TEventArgs eventArgs)
     where TSender : allows ref struct
     where TEventArgs : allows ref struct;
#endif