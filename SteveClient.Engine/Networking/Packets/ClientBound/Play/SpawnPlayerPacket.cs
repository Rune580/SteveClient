using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class SpawnPlayerPacket : ClientBoundPacket
{
    public int EntityId;
    public byte[] UuidBytes;
    public Vector3d Position;
    public byte Yaw;
    public byte Pitch;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        UuidBytes = packetBuffer.ReadByteArray(16);
        Position = packetBuffer.ReadVector3d();
        Yaw = packetBuffer.ReadUnsignedByte();
        Pitch = packetBuffer.ReadUnsignedByte();
    }
}