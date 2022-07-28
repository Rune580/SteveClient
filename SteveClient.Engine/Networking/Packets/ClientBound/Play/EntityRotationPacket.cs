using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class EntityRotationPacket : ClientBoundPacket
{
    public int EntityId;
    public byte Yaw;
    public byte Pitch;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        Yaw = packetBuffer.ReadUnsignedByte();
        Pitch = packetBuffer.ReadUnsignedByte();
    }
}