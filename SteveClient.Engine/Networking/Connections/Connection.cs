using System.Net.Sockets;
using SteveClient.Engine.Networking.Packets;
using SteveClient.Engine.Networking.Packets.ClientBound;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Connections;

public class Connection : IDisposable
{
    private readonly NetworkStream _networkStream;
    private readonly Queue<IServerBoundPacket> _serverBoundPackets = new();
    private readonly CancellationTokenSource _cts;

    private bool _readActive;
    private bool _writeActive;

    public ConnectionState CurrentState { get; private set; }
    public int CompressionThreshold = -1;

    public Connection(TcpClient client)
    {
        _networkStream = client.GetStream();
        _cts = new CancellationTokenSource();

        ThreadPool.QueueUserWorkItem(ReadThread, _cts.Token);
        ThreadPool.QueueUserWorkItem(WriteThread, _cts.Token);

        _readActive = true;
        _writeActive = true;
    }

    private void ReadThread(object? obj)
    {
        CancellationToken token = (CancellationToken)obj!;

        while (_networkStream.CanRead)
        {
            if (token.IsCancellationRequested)
                break;

            InPacketBuffer packetBuffer = new InPacketBuffer(_networkStream);
            
            ClientBoundPacket.FromPacketBuffer(packetBuffer);
        }

        _readActive = false;
    }

    private void WriteThread(object? obj)
    {
        CancellationToken token = (CancellationToken)obj!;

        while (_networkStream.CanWrite)
        {
            if (token.IsCancellationRequested)
                break;

            if (_serverBoundPackets.Count == 0)
                continue;

            var packet = _serverBoundPackets.Dequeue();
            
            _networkStream.Write(packet.Flush());
        }

        _writeActive = false;
    }

    public void EnqueuePacket(IServerBoundPacket packet)
    {
        _serverBoundPackets.Enqueue(packet);
    }

    public void UpdateConnectionState(ConnectionState state)
    {
        CurrentState = state;
    }

    public void Dispose()
    {
        _cts.Cancel();

        while (_readActive || _writeActive)
            Thread.Sleep(100);

        _cts.Dispose();
    }
}