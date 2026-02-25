namespace LXGaming.Common.Threading.Tasks.Models;

public class RemovedEventArgs<TKey> : EventArgs {

    public required TKey Key { get; init; }
}