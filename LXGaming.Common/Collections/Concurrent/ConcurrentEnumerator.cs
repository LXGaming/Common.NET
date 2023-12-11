using System.Collections;

namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentEnumerator<T> : IEnumerator<T> {

    private readonly IEnumerator<T> _enumerator;
    private readonly ReaderWriterLockSlim _lock;
    private bool _disposed;

    public ConcurrentEnumerator(IEnumerable<T> enumerable, ReaderWriterLockSlim @lock) {
        _enumerator = enumerable.GetEnumerator();
        _lock = @lock;
        _lock.EnterReadLock();
    }

    public bool MoveNext() => _enumerator.MoveNext();

    public void Reset() => _enumerator.Reset();

#nullable disable
    object IEnumerator.Current => Current;
#nullable restore

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (_disposed) {
            return;
        }

        if (disposing) {
            _enumerator.Dispose();
            _lock.ExitReadLock();
        }

        _disposed = true;
    }

    public T Current => _enumerator.Current;
}