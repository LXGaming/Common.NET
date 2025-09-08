namespace LXGaming.Common.Threading.Tasks;

public class CancellableTask(Func<CancellableTaskContext, Task> func) : IAsyncDisposable {

    public CancellationToken CancelToken => _cancelSource.Token;
    public CancellationToken StopToken => _stopSource.Token;
    public CancellableTaskStatus Status => _status;

    private readonly CancellationTokenSource _cancelSource = new();
    private readonly CancellationTokenSource _stopSource = new();
    private readonly SemaphoreSlim _lock = new(1, 1);
    private volatile CancellableTaskStatus _status = CancellableTaskStatus.Created;
    private volatile Task? _task;
    private bool _disposed;

    public async Task StartAsync() {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_task != null) {
            await _task.ConfigureAwait(false);
            return;
        }

        await _lock.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        try {
            if (_task == null) {
                CancelToken.ThrowIfCancellationRequested();
                _status = CancellableTaskStatus.Started;

                try {
                    _task = func(new CancellableTaskContext(CancelToken, StopToken));
                } catch (Exception ex) {
                    _task = Task.FromException(ex);
                }
            }
        } finally {
            _lock.Release();
        }

        await _task.ConfigureAwait(false);
    }

    public async Task StopAsync() {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (CancelToken.IsCancellationRequested) {
            if (_task != null) {
                await _task.ConfigureAwait(false);
            }

            return;
        }

        await _lock.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        try {
            if (!CancelToken.IsCancellationRequested) {
                _status = CancellableTaskStatus.Stopped;
                await _stopSource.CancelAsync().ConfigureAwait(false);
                await _cancelSource.CancelAsync().ConfigureAwait(false);
            }
        } finally {
            _lock.Release();
        }

        if (_task != null) {
            await _task.ConfigureAwait(false);
        }
    }

    private async Task CancelAsync() {
        if (CancelToken.IsCancellationRequested) {
            if (_task != null) {
                await _task.ConfigureAwait(false);
            }

            return;
        }

        await _lock.WaitAsync(CancellationToken.None).ConfigureAwait(false);
        try {
            if (!CancelToken.IsCancellationRequested) {
                _status = CancellableTaskStatus.Cancelled;
                await _cancelSource.CancelAsync().ConfigureAwait(false);
            }
        } finally {
            _lock.Release();
        }

        if (_task != null) {
            await _task.ConfigureAwait(false);
        }
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync() {
        await DisposeAsync(true).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsync(bool disposing) {
        if (_disposed) {
            return;
        }

        if (disposing) {
            try {
                await CancelAsync().ConfigureAwait(false);
            } catch (Exception) {
                // no-op
            }

            _task?.Dispose();
            _lock.Dispose();
            _stopSource.Dispose();
            _cancelSource.Dispose();
        }

        _disposed = true;
    }
}