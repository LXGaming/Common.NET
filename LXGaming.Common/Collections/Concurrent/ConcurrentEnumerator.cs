using System.Collections;

namespace LXGaming.Common.Collections.Concurrent;

public class ConcurrentEnumerator<T> : IEnumerator<T> {

    private readonly IEnumerator<T> _enumerator;
    private readonly ReaderWriterLockSlim _lock;
    private bool _disposed;

    public ConcurrentEnumerator(IEnumerable<T> enumerable, ReaderWriterLockSlim @lock) {
        _enumerator = enumerable.GetEnumerator();
        _lock = @lock;

        try {
            _lock.EnterReadLock();
        } catch (Exception) {
            _enumerator.Dispose();
            throw;
        }
    }

    /// <inheritdoc />
    public bool MoveNext() => _enumerator.MoveNext();

#nullable disable
    /// <inheritdoc />
    object IEnumerator.Current => Current;
#nullable restore

    /// <inheritdoc />
    public void Reset() => _enumerator.Reset();

    /// <inheritdoc />
    public T Current => _enumerator.Current;

    /// <inheritdoc />
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if (_disposed) {
            return;
        }

        _disposed = true;

        if (disposing) {
            _enumerator.Dispose();
            _lock.ExitReadLock();
        }
    }
}