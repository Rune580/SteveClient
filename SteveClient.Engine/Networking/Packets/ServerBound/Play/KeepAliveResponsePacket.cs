using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Play;

public class KeepAliveResponsePacket : ServerBoundPacket
{
    public long KeepAliveId { get; }

    public KeepAliveResponsePacket(long keepAliveId)
    {
        KeepAliveId = keepAliveId;
    }
    
    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteLong(KeepAliveId);
    }
}