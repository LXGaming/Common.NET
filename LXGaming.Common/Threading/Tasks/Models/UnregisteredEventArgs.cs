namespace LXGaming.Common.Threading.Tasks.Models;

public class UnregisteredEventArgs<TKey> : EventArgs {

    public required TKey Key { get; init; }
}