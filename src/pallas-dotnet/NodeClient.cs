using PallasDotnet.Models;

namespace PallasDotnet;

public class ChainSyncNextResponseEventArgs(NextResponse nextResponse) : EventArgs
{
    public NextResponse NextResponse { get; } = nextResponse;
}

public class NodeClient
{
    private PallasDotnetRs.PallasDotnetRs.NodeClientWrapper? _nodeClient;
    private string _socketPath = string.Empty;
    private ulong _magicNumber = 0;
    private byte[] _lastHash = [];
    private ulong _lastSlot = 0;

    public bool IsConnected => _nodeClient != null;
    public bool IsSyncing { get; private set; }
    public bool ShouldRecoonect { get; set; } = true;

    public event EventHandler<ChainSyncNextResponseEventArgs>? ChainSyncNextResponse;
    public event EventHandler? Disconnected;
    public event EventHandler? Reconnected;

    public async Task<Point> ConnectAsync(string socketPath, ulong magicNumber)
    {
        _magicNumber = magicNumber;
        _socketPath = socketPath;

        return await Task.Run(() =>
        {
            _nodeClient = PallasDotnetRs.PallasDotnetRs.Connect(socketPath, magicNumber);

            if (_nodeClient is null)
            {
                throw new Exception("Failed to connect to node");
            }

            var pallasPoint = PallasDotnetRs.PallasDotnetRs.GetTip(_nodeClient.Value);
            return Utils.MapPallasPoint(pallasPoint);
        });
    }

    public async Task StartChainSyncAsync(Point? intersection = null)
    {
        if (_nodeClient is null)
        {
            throw new Exception("Not connected to node");
        }

        if (intersection is not null)
        {
            await Task.Run(() =>
            {
                PallasDotnetRs.PallasDotnetRs.FindIntersect(_nodeClient.Value, new PallasDotnetRs.PallasDotnetRs.Point
                {
                    slot = intersection.Slot,
                    hash = new List<byte>(intersection.Hash.Bytes)
                });
            });
        }

        _ = Task.Run(() =>
        {
            IsSyncing = true;
            while (IsSyncing)
            {
                var nextResponseRs = PallasDotnetRs.PallasDotnetRs.ChainSyncNext(_nodeClient.Value);
                if ((NextResponseAction)nextResponseRs.action == NextResponseAction.Error)
                {
                    if (ShouldRecoonect)
                    {
                        _nodeClient = PallasDotnetRs.PallasDotnetRs.Connect(_socketPath, _magicNumber);
                        PallasDotnetRs.PallasDotnetRs.FindIntersect(_nodeClient.Value, new PallasDotnetRs.PallasDotnetRs.Point
                        {
                            slot = _lastSlot,
                            hash = [.. _lastHash]
                        });
                        Reconnected?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        IsSyncing = false;
                        Disconnected?.Invoke(this, EventArgs.Empty);
                    }
                }
                else if ((NextResponseAction)nextResponseRs.action == NextResponseAction.Await)
                {
                    ChainSyncNextResponse?.Invoke(this, new(new(
                        NextResponseAction.Await,
                        default!,
                        default!
                    )));
                }
                else
                {
                    var nextResponse = Utils.MapPallasNextResponse(nextResponseRs);
                    _lastHash = nextResponse.Block.Hash.Bytes;
                    _lastSlot = nextResponse.Block.Slot;
                    ChainSyncNextResponse?.Invoke(this, new(nextResponse));
                }
            }
        });
    }

    public void StopSync()
    {
        IsSyncing = false;
    }

    public Task DisconnectAsync()
    {
        if (_nodeClient is null)
        {
            throw new Exception("Not connected to node");
        }
        return Task.Run(() => PallasDotnetRs.PallasDotnetRs.Disconnect(_nodeClient.Value));
    }
}
