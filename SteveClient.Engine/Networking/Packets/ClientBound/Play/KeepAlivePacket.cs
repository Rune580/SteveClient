using SteveClient.Engine.Networking.Packets.ServerBound.Play;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class KeepAlivePacket : ClientBoundPacket
{
    public override void Read(in InPacketBuffer packetBuffer)
    {
        long keepAliveId = packetBuffer.ReadLong();
        
        new KeepAliveResponsePacket(keepAliveId).SendToServer();
    }
}