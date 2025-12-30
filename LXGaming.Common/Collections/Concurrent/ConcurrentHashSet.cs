namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentHashSet<T> : ConcurrentSet<T> {

    public ConcurrentHashSet() : base(new HashSet<T>()) {
        // no-op
    }

    public ConcurrentHashSet(int capacity) : base(new HashSet<T>(capacity)) {
        // no-op
    }

    public ConcurrentHashSet(IEqualityComparer<T> comparer) : base(new HashSet<T>(comparer)) {
        // no-op
    }

    public ConcurrentHashSet(int capacity, IEqualityComparer<T> comparer) : base(new HashSet<T>(capacity, comparer)) {
        // no-op
    }
}