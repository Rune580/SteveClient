using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Packets.ClientBound;
using SteveClient.Engine.Networking.Packets.ClientBound.Login;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Engine.Networking.Packets.ServerBound.Handshaking;
using SteveClient.Engine.Networking.Packets.ServerBound.Login;
using SteveClient.Engine.Networking.Packets.ServerBound.Play;

namespace SteveClient.Engine.Networking.Packets;

public static class PacketRegistry
{
    public static readonly Dictionary<Type, int> ServerBoundPacketMap = new();
    public static readonly Dictionary<ConnectionState, Dictionary<int, Type>> ClientBoundPacketMap = new();

    static PacketRegistry()
    {
        RegisterServerBoundPackets();
        RegisterClientBoundPackets();
    }

    private static void RegisterServerBoundPackets()
    {
        // Handshake
        ServerBoundPacketMap[typeof(BeginHandshakePacket)] = 0x00;
        
        // Login
        ServerBoundPacketMap[typeof(LoginStartPacket)] = 0x00;
        
        // Play
        ServerBoundPacketMap[typeof(TeleportConfirmPacket)] = 0x00;
        ServerBoundPacketMap[typeof(ClientStatusPacket)] = 0x06;
        ServerBoundPacketMap[typeof(ClientSettingsPacket)] = 0x07;
        ServerBoundPacketMap[typeof(KeepAliveResponsePacket)] = 0x11;
    }

    private static void RegisterClientBoundPackets()
    {
        var handshake = new Dictionary<int, Type>();
        var login = new Dictionary<int, Type>();
        var play = new Dictionary<int, Type>();

        login[0x00] = typeof(DisconnectPacket);
        login[0x02] = typeof(LoginSuccessPacket);
        login[0x03] = typeof(SetCompressionPacket);

        play[0x17] = typeof(DisconnectPacket);
        play[0x1E] = typeof(KeepAlivePacket);
        play[0x1F] = typeof(ChunkDataAndUpdateLightPacket);
        play[0x23] = typeof(JoinGamePacket);
        play[0x36] = typeof(PlayerPositionAndLookPacket);

        ClientBoundPacketMap[ConnectionState.Handshaking] = handshake;
        ClientBoundPacketMap[ConnectionState.Login] = login;
        ClientBoundPacketMap[ConnectionState.Play] = play;
    }

    public static Type GetClientBoundPacket(int packetId, ConnectionState state)
    {
        return ClientBoundPacketMap[state].TryGetValue(packetId, out var packetType) ? packetType : typeof(UnknownPacket);
    }

    public static Type GetClientBoundPacket(int packetId)
    {
        return GetClientBoundPacket(packetId, MinecraftNetworkingClient.Instance!.Connection!.CurrentState);
    }
}