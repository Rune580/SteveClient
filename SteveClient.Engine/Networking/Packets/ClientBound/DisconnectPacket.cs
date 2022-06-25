using System.Text;
using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound;

public class DisconnectPacket : ClientBoundPacket
{
    public string ChatJson { get; set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        byte[] bytes = packetBuffer.ReadRest();

        ChatJson = Encoding.UTF8.GetString(bytes);
        
        MinecraftNetworkingClient.Instance!.Disconnect();
    }
}