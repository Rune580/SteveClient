using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Exceptions;
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
    private static readonly Dictionary<ConnectionState, Dictionary<int, ClientBoundPacketRegistryEntry>> ClientBoundPacketMap = new();

    static PacketRegistry()
    {
        RegisterServerBoundPackets();
        
        RegisterClientBoundHandshakePackets();
        RegisterClientBoundLoginPackets();
        RegisterClientBoundPlayPackets();
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
        ServerBoundPacketMap[typeof(SyncPlayerPositionPacket)] = 0x13;
    }
    
    private static void RegisterClientBoundHandshakePackets()
    {
        var handshake = new Dictionary<int, ClientBoundPacketRegistryEntry>();
        
        ClientBoundPacketMap[ConnectionState.Handshaking] = handshake;
    }

    private static void RegisterClientBoundLoginPackets()
    {
        var login = new Dictionary<int, ClientBoundPacketRegistryEntry>();
        
        login.Register<DisconnectPacket>(0x00);
        login.Register<LoginSuccessPacket>(0x02);
        login.Register<SetCompressionPacket>(0x03);

        ClientBoundPacketMap[ConnectionState.Login] = login;
    }
    
    private static void RegisterClientBoundPlayPackets()
    {
        var play = new Dictionary<int, ClientBoundPacketRegistryEntry>();
        
        play.Register<SpawnPlayerPacket>(0x02);
        play.Register<BlockUpdatePacket>(0x09);
        play.Register<DisconnectPacket>(0x17);
        play.Register<KeepAlivePacket>(0x1E);
        play.Register<ChunkDataAndUpdateLightPacket>(0x1F);
        play.Register<UpdateLightPacket>(0x22);
        play.Register<JoinGamePacket>(0x23);
        play.Register<EntityPositionPacket>(0x26);
        play.Register<EntityPositionAndRotationPacket>(0x27);
        play.Register<EntityRotationPacket>(0x28);
        play.Register<PlayerPositionAndLookPacket>(0x36);
        play.Register<SetEntityVelocityPacket>(0x4F);
        play.Register<TeleportEntityPacket>(0x63);

        ClientBoundPacketMap[ConnectionState.Play] = play;
    }

    public static ClientBoundPacketRegistryEntry GetClientBoundPacketRegistryEntry(int packetId, ConnectionState state)
    {
        if (!ClientBoundPacketMap[state].TryGetValue(packetId, out var entry))
            throw new PacketNotFoundException();

        return entry;
    }
    
    public static bool TryGetClientBoundPacketRegistryEntry(int packetId, out ClientBoundPacketRegistryEntry registryEntry)
    {
        var state = MinecraftNetworkingClient.Instance!.Connection!.CurrentState;

        return ClientBoundPacketMap[state].TryGetValue(packetId, out registryEntry);
    }

    public static Type GetClientBoundPacketType(int packetId, ConnectionState state)
    {
        return ClientBoundPacketMap[state].TryGetValue(packetId, out var entry) ? entry.PacketType : typeof(UnknownPacket);
    }

    public static Type GetClientBoundPacketType(int packetId)
    {
        return GetClientBoundPacketType(packetId, MinecraftNetworkingClient.Instance!.Connection!.CurrentState);
    }

    public static int GetClientBoundPacketId<TPacket>(ConnectionState state) where TPacket : ClientBoundPacket
    {
        var results = ClientBoundPacketMap[state].Where(pair => pair.Value.PacketType == typeof(TPacket)).Select(pair => pair.Key).ToArray();

        if (results.Length != 1)
            throw new PacketNotFoundException();
        
        return results[0];
    }

    public static int GetClientBoundPacketId<TPacket>() where TPacket : ClientBoundPacket
    {
        return GetClientBoundPacketId<TPacket>(MinecraftNetworkingClient.Instance!.Connection!.CurrentState);
    }

    public static void RegisterClientBoundPacketListener<TPacket>(ConnectionState state, Action<ClientBoundPacket> listener) where TPacket : ClientBoundPacket
    {
        int packetId = GetClientBoundPacketId<TPacket>(state);
        var entry = GetClientBoundPacketRegistryEntry(packetId, state);

        entry.PacketReceived += listener;
    }

    public class ClientBoundPacketRegistryEntry
    {
        public readonly Type PacketType;
        public Action<ClientBoundPacket>? PacketReceived;

        public ClientBoundPacketRegistryEntry(Type packetType)
        {
            PacketType = packetType;
        }
    }

    private static void Register<TPacket>(this Dictionary<int, ClientBoundPacketRegistryEntry> map, int packetId) where TPacket : ClientBoundPacket
    {
        map[packetId] = new ClientBoundPacketRegistryEntry(typeof(TPacket));
    }
}