using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class TeleportEntityPacket : ClientBoundPacket
{
    public int EntityId { get; private set; }
    public Vector3d Position { get; private set; }
    public byte Yaw { get; private set; }
    public byte Pitch { get; private set; }
    public bool OnGround { get; private set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        Position = packetBuffer.ReadVector3d();
        Yaw = packetBuffer.ReadUnsignedByte();
        Pitch = packetBuffer.ReadUnsignedByte();
        OnGround = packetBuffer.ReadBool();
    }
}