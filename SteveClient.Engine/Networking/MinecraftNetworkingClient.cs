using System.Net;
using System.Net.Sockets;
using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Packets.ServerBound.Handshaking;
using SteveClient.Engine.Networking.Packets.ServerBound.Login;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking;

public class MinecraftNetworkingClient : IDisposable
{
    public static MinecraftNetworkingClient? Instance;
    
    private readonly TcpClient _client;
    private Connection? _connection;

    public MinecraftNetworkingClient()
    {
        _client = new TcpClient();

        Instance = this;
    }

    public void Connect(string host, ushort port)
    {
        host = "127.0.0.1";
        
        IPAddress ip = IPAddress.Parse(host);
        
        ConnectAsync(ip, port).ConfigureAwait(true).GetAwaiter().GetResult();
    }

    public void Disconnect()
    {
        if (_connection is null)
            Console.WriteLine("Not connected to any server!");

        _connection!.Dispose();
        
        _client.Close();
    }

    private async Task ConnectAsync(IPAddress ip, ushort port)
    {
        Console.WriteLine("Connecting...");
        await _client.ConnectAsync(ip, port);

        while (!_client.Connected)
        {
            await Task.Delay(100);
            Console.WriteLine("Waiting...");
        }

        _connection = new Connection(_client);
        
        _connection.UpdateConnectionState(ConnectionState.Handshaking);
        new BeginHandshakePacket(ip.ToString(), port, ConnectionState.Login).SendToServer();
        _connection.UpdateConnectionState(ConnectionState.Login);
        new LoginStartPacket("TestBot").SendToServer();
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public Connection? Connection => _connection;
}