using System.Collections;
using System.Collections.Concurrent;
using LXGaming.Common.Event;
using LXGaming.Common.Threading.Tasks.Models;

namespace LXGaming.Common.Threading.Tasks;

public class CancellableTaskCollection<TKey> : IEnumerable<TKey>, IAsyncDisposable where TKey : notnull {

    public event AsyncEventHandler<RegisteredEventArgs<TKey>>? Registered;
    public event AsyncEventHandler<UnhandledExceptionEventArgs<TKey>>? UnhandledException;
    public event AsyncEventHandler<UnregisteredEventArgs<TKey>>? Unregistered;

    public int Count => _cancellableTasks.Count;
    public ICollection<TKey> Keys => _cancellableTasks.Keys;

    private readonly ConcurrentDictionary<TKey, CancellableTask> _cancellableTasks = [];
    private volatile bool _disposed;

    public async Task<bool> RegisterAsync(TKey key, Func<CancellableTaskContext, Task> func) {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_cancellableTasks.ContainsKey(key)) {
            return false;
        }

        var cancellableTask = new CancellableTask(async context => {
            try {
                await func(context).ConfigureAwait(false);
            } catch (Exception ex) {
                try {
                    await UnhandledException.InvokeAsync(this, new UnhandledExceptionEventArgs<TKey> {
                        Key = key,
                        Exception = ex,
                    }).ConfigureAwait(false);
                } catch (Exception) {
                    // no-op
                }
            } finally {
                await UnregisterInternalAsync(key).ConfigureAwait(false);
            }
        });
        bool registered;
        try {
            registered = _cancellableTasks.TryAdd(key, cancellableTask);
        } catch (Exception) {
            registered = false;
        }

        if (!registered) {
            await cancellableTask.DisposeAsync().ConfigureAwait(false);
            return false;
        }

        // If dispose was called while the CancellableTask was being added we need to remove and dispose of it.
        if (_disposed) {
            _cancellableTasks.TryRemove(key, out _);
            await cancellableTask.DisposeAsync().ConfigureAwait(false);
            return false;
        }

        try {
            await Registered.InvokeAsync(this, new RegisteredEventArgs<TKey> {
                Key = key
            }).ConfigureAwait(false);
        } catch (Exception) {
            // no-op
        }

        Task task;
        try {
            task = cancellableTask.StartAsync();
        } catch (Exception) {
            await UnregisterInternalAsync(key).ConfigureAwait(false);
            await cancellableTask.DisposeAsync().ConfigureAwait(false);
            return false;
        }

        _ = task.ContinueWith(async _ => {
            await UnregisterInternalAsync(key).ConfigureAwait(false);
            await cancellableTask.DisposeAsync().ConfigureAwait(false);
        }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
        return true;
    }

    public async Task<bool> UnregisterAsync(TKey key, bool stop = true) {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (!_cancellableTasks.TryGetValue(key, out var cancellableTask)) {
            return false;
        }

        try {
            if (stop) {
                await cancellableTask.StopAsync().ConfigureAwait(false);
            }
        } finally {
            await cancellableTask.DisposeAsync().ConfigureAwait(false);
        }

        return true;
    }

    public Task UnregisterAllAsync(bool stop = true) {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return UnregisterAllAsync(_ => true, stop);
    }

    public async Task UnregisterAllAsync(Predicate<TKey> match, bool stop = true) {
        ObjectDisposedException.ThrowIf(_disposed, this);

        List<Exception>? exceptions = null;
        foreach (var pair in _cancellableTasks) {
            if (!match(pair.Key)) {
                continue;
            }

            try {
                if (stop) {
                    await pair.Value.StopAsync().ConfigureAwait(false);
                }
            } catch (Exception ex) {
                exceptions ??= [];
                exceptions.Add(ex);
            } finally {
                await pair.Value.DisposeAsync().ConfigureAwait(false);
            }
        }

        if (exceptions != null) {
            throw new AggregateException(exceptions);
        }
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    /// <inheritdoc />
    public IEnumerator<TKey> GetEnumerator() {
        return _cancellableTasks.Select(pair => pair.Key).GetEnumerator();
    }

    public bool Contains(TKey key) {
        return _cancellableTasks.ContainsKey(key);
    }

    private async Task UnregisterInternalAsync(TKey key) {
        if (!_cancellableTasks.TryRemove(key, out _)) {
            return;
        }

        try {
            await Unregistered.InvokeAsync(this, new UnregisteredEventArgs<TKey> {
                Key = key
            }).ConfigureAwait(false);
        } catch (Exception) {
            // no-op
        }
    }

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore() {
        if (_disposed) {
            return;
        }

        _disposed = true;

        foreach (var pair in _cancellableTasks) {
            await pair.Value.DisposeAsync().ConfigureAwait(false);
        }
    }
}