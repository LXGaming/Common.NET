namespace LXGaming.Common.Threading.Tasks;

public readonly struct CancellableTaskContext(CancellationToken cancelToken, CancellationToken stopToken) {

    public CancellationToken CancelToken { get; } = cancelToken;

    public CancellationToken StopToken { get; } = stopToken;
}