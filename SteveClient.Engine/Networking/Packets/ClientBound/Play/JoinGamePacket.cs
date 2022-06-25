using OpenTK.Mathematics;
using SmartNbt.Tags;
using SteveClient.Engine.Engines;
using SteveClient.Engine.Networking.Packets.ServerBound.Play;
using SteveClient.Engine.Networking.Protocol;
using static SteveClient.Engine.Networking.Packets.ServerBound.Play.ClientStatusPacket;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class JoinGamePacket : ClientBoundPacket
{
    public int EntityId { get; private set; }
    public bool IsHardcore { get; private set; }
    public byte GameMode { get; private set; }
    public sbyte PrevGameMode { get; private set; }
    public int WorldCount => DimensionNames.Length;
    public string[] DimensionNames { get; private set; }
    public NbtCompound RegistryCodec { get; private set; }
    public string DimensionType { get; private set; }
    public string DimensionName { get; private set; }
    public long HashedSeed { get; private set; }
    public int MaxPlayers { get; private set; }
    public int ViewDistance { get; private set; }
    public int SimulationDistance { get; private set; }
    public bool ReducedDebugInfo { get; private set; }
    public bool EnableRespawnScreen { get; private set; }
    public bool IsDebug { get; private set; }
    public bool IsFlat { get; private set; }
    public bool HasDeathLocation { get; private set; }
    public string? DeathDimensionName { get; private set; }
    public Vector3i? DeathLocation { get; private set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadInt();
        IsHardcore = packetBuffer.ReadBool();
        GameMode = packetBuffer.ReadUnsignedByte();
        PrevGameMode = packetBuffer.ReadSignedByte();
        DimensionNames = packetBuffer.ReadIdentifierArray();
        RegistryCodec = packetBuffer.ReadNbtCompound();
        DimensionType = packetBuffer.ReadString();
        DimensionName = packetBuffer.ReadString();
        HashedSeed = packetBuffer.ReadLong();
        MaxPlayers = packetBuffer.ReadVarInt();
        ViewDistance = packetBuffer.ReadVarInt();
        SimulationDistance = packetBuffer.ReadVarInt();
        ReducedDebugInfo = packetBuffer.ReadBool();
        EnableRespawnScreen = packetBuffer.ReadBool();
        IsDebug = packetBuffer.ReadBool();
        IsFlat = packetBuffer.ReadBool();
        HasDeathLocation = packetBuffer.ReadBool();

        if (HasDeathLocation)
        {
            DeathDimensionName = packetBuffer.ReadString();
            // Todo read position
        }
        
        new ClientSettingsPacket().SendToServer();
        new ClientStatusPacket(ClientStatusAction.PerformRespawn).SendToServer();
        
        SpawnPlayerEntityEngine.JoinGamePackets.Enqueue(this);
    }
}