namespace LXGaming.Common.Threading.Tasks;

public class CancellableTask : IAsyncDisposable {

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    public bool Stopped { get; private set; }

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task? _task;
    private bool _disposed;

    public Task StartAsync(Func<Task> function) {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_task != null) {
            throw new InvalidOperationException("Task already started.");
        }

        return _task = function();
    }

    public async Task StopAsync() {
        ObjectDisposedException.ThrowIf(_disposed, this);

        Stopped = true;
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
        if (_task != null) {
            await _task.ConfigureAwait(false);
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
                await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
                if (_task != null) {
                    await _task.ConfigureAwait(false);
                }
            } catch (Exception) {
                // no-op
            }

            _cancellationTokenSource.Dispose();
        }

        _disposed = true;
    }
}