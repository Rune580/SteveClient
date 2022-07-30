using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class SetEntityVelocityPacket : ClientBoundPacket
{
    public int EntityId { get; private set; }
    public Vector3d Velocity { get; private set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        EntityId = packetBuffer.ReadVarInt();
        Velocity = packetBuffer.ReadVelocity();
    }
}