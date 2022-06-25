using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets;

public interface IClientBoundPacket
{
    void Read(in InPacketBuffer packetBuffer);
}