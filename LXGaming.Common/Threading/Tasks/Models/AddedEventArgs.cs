namespace LXGaming.Common.Threading.Tasks.Models;

public class AddedEventArgs<TKey> : EventArgs {

    public required TKey Key { get; init; }
}