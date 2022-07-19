using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class EntityPositionPacket : ClientBoundPacket
{
    public int EntityId;
    public Vector3i Delta;
    public bool OnGround;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        Delta = packetBuffer.ReadDelta();
        OnGround = packetBuffer.ReadBool();
    }
}