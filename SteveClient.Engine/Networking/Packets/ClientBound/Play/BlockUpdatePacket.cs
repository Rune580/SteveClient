using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class BlockUpdatePacket : ClientBoundPacket
{
    public Vector3i BlockPos { get; private set; }
    public int BlockStateId { get; private set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        BlockPos = packetBuffer.ReadPosition();
        BlockStateId = packetBuffer.ReadVarInt();
    }
}