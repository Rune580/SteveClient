using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Login;

public class SetCompressionPacket : ClientBoundPacket
{
    public int Threshold { get; set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Threshold = packetBuffer.ReadVarInt();

        MinecraftNetworkingClient.Instance!.Connection!.CompressionThreshold = Threshold;
    }
}