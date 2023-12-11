namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentSortedSet<T> : ConcurrentSet<T> {

    public ConcurrentSortedSet()
        : base(new SortedSet<T>()) {
    }

    public ConcurrentSortedSet(IComparer<T> comparer)
        : base(new SortedSet<T>(comparer)) {
    }
}