namespace LXGaming.Common.Threading.Tasks.Models;

public class UnhandledExceptionEventArgs<TKey> : EventArgs {

    public required TKey Key { get; init; }

    public required Exception Exception { get; init; }
}