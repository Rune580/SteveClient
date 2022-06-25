namespace SteveClient.Engine.Networking.Connections;

public enum ConnectionState
{
    Handshaking,
    Status,
    Login,
    Play,
    None
}