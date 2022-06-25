using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound;

public class UnknownPacket : ClientBoundPacket
{
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Console.WriteLine($"Unknown packet of id {PacketId:x2} received!");
    }
}