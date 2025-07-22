namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentSet<T>(ISet<T> set) : ConcurrentCollection<ISet<T>, T>(set), ISet<T> {

    /// <inheritdoc />
    public new bool Add(T item) {
        Lock.EnterWriteLock();
        try {
            return Collection.Add(item);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void UnionWith(IEnumerable<T> other) {
        Lock.EnterWriteLock();
        try {
            Collection.UnionWith(other);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void IntersectWith(IEnumerable<T> other) {
        Lock.EnterWriteLock();
        try {
            Collection.IntersectWith(other);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void ExceptWith(IEnumerable<T> other) {
        Lock.EnterWriteLock();
        try {
            Collection.ExceptWith(other);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void SymmetricExceptWith(IEnumerable<T> other) {
        Lock.EnterWriteLock();
        try {
            Collection.SymmetricExceptWith(other);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public bool IsSubsetOf(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.IsSubsetOf(other);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool IsSupersetOf(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.IsSupersetOf(other);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool IsProperSupersetOf(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.IsProperSupersetOf(other);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool IsProperSubsetOf(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.IsProperSubsetOf(other);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool Overlaps(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.Overlaps(other);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool SetEquals(IEnumerable<T> other) {
        Lock.EnterReadLock();
        try {
            return Collection.SetEquals(other);
        } finally {
            Lock.ExitReadLock();
        }
    }
}