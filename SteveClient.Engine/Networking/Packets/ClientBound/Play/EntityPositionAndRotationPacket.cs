using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class EntityPositionAndRotationPacket : ClientBoundPacket
{
    public int EntityId;
    public Vector3d Delta;
    public byte Yaw;
    public byte Pitch;
    public bool OnGround;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        Delta = packetBuffer.ReadDelta();
        Yaw = packetBuffer.ReadUnsignedByte();
        Pitch = packetBuffer.ReadUnsignedByte();
        OnGround = packetBuffer.ReadBool();
    }
}