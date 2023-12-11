using System.Collections;

namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentCollection<TCollection, TItem> : ICollection<TItem>, IDisposable where TCollection : ICollection<TItem> {

    protected readonly TCollection Collection;
    protected readonly ReaderWriterLockSlim Lock;
    private bool _disposed;

    public ConcurrentCollection(TCollection collection) {
        Collection = collection;
        Lock = new ReaderWriterLockSlim();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public IEnumerator<TItem> GetEnumerator() => new ConcurrentEnumerator<TItem>(Collection, Lock);

    public void Add(TItem item) {
        Lock.EnterWriteLock();
        try {
            Collection.Add(item);
        } finally {
            Lock.ExitWriteLock();
        }
    }

    public void Clear() {
        Lock.EnterWriteLock();
        try {
            Collection.Clear();
        } finally {
            Lock.ExitWriteLock();
        }
    }

    public bool Contains(TItem item) {
        Lock.EnterReadLock();
        try {
            return Collection.Contains(item);
        } finally {
            Lock.ExitReadLock();
        }
    }

    public void CopyTo(TItem[] array, int arrayIndex) {
        Lock.EnterReadLock();
        try {
            Collection.CopyTo(array, arrayIndex);
        } finally {
            Lock.ExitReadLock();
        }
    }

    public bool Remove(TItem item) {
        Lock.EnterWriteLock();
        try {
            return Collection.Remove(item);
        } finally {
            Lock.ExitWriteLock();
        }
    }

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

    public bool IsReadOnly => false;

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