namespace LXGaming.Common.Threading.Tasks.Models;

public class RegisteredEventArgs<TKey> : EventArgs {

    public required TKey Key { get; init; }
}