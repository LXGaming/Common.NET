using System.Collections;

namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentCollection<TCollection, TItem>(TCollection collection)
    : ICollection<TItem>, IDisposable where TCollection : ICollection<TItem> {

    protected readonly TCollection Collection = collection;
    protected readonly ReaderWriterLockSlim Lock = new();
    private bool _disposed;

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<TItem> GetEnumerator() => new ConcurrentEnumerator<TItem>(Collection, Lock);

    /// <inheritdoc />
    public int Count {
        get {
            Lock.EnterReadLock();
            try {
                return Collection.Count;
            } finally {
                Lock.ExitReadLock();
            }
        }
    }

    /// <inheritdoc />
    public bool IsReadOnly => false;

    /// <inheritdoc />
    public void Add(TItem item) {
        Lock.EnterWriteLock();
        try {
            Collection.Add(item);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void Clear() {
        Lock.EnterWriteLock();
        try {
            Collection.Clear();
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public bool Contains(TItem item) {
        Lock.EnterReadLock();
        try {
            return Collection.Contains(item);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public void CopyTo(TItem[] array, int arrayIndex) {
        Lock.EnterReadLock();
        try {
            Collection.CopyTo(array, arrayIndex);
        } finally {
            Lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public bool Remove(TItem item) {
        Lock.EnterWriteLock();
        try {
            return Collection.Remove(item);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (_disposed) {
            return;
        }

        if (disposing) {
            Lock.Dispose();
        }

        _disposed = true;
    }
}