namespace LXGaming.Common.Threading.Tasks;

public class CancellableTask(Func<CancellationToken, Task> func) : IAsyncDisposable {

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    public CancellableTaskStatus Status => _status;

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly object _lock = new();
    private volatile CancellableTaskStatus _status = CancellableTaskStatus.Created;
    private volatile Task? _task;
    private bool _disposed;

    public Task StartAsync() {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_task != null) {
            return _task;
        }

        lock (_lock) {
            if (_task != null) {
                return _task;
            }

            CancellationToken.ThrowIfCancellationRequested();

            Task task;
            try {
                task = func(CancellationToken);
            } catch (Exception ex) {
                task = Task.FromException(ex);
            }

            _task = task;
            _status = CancellableTaskStatus.Started;
        }

        return _task;
    }

    public async Task StopAsync() {
        ObjectDisposedException.ThrowIf(_disposed, this);

        try {
            await CancelAsync().ConfigureAwait(false);
        } finally {
            _status = CancellableTaskStatus.Stopped;
        }
    }

    private async Task CancelAsync() {
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);

        Task? task;
        if (_task != null) {
            task = _task;
        } else {
            lock (_lock) {
                task = _task;
            }
        }

        if (task != null) {
            await task.ConfigureAwait(false);
        }
    }

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
            _cancellationTokenSource.Dispose();
        }

        _disposed = true;
    }
}