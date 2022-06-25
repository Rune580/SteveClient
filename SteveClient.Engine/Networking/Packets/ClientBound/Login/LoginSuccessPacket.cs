using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Login;

public class LoginSuccessPacket : ClientBoundPacket
{
    public byte[] UuidBytes { get; private set; }
    public string Username { get; private set; }
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        UuidBytes = packetBuffer.ReadByteArray(16);
        Username = packetBuffer.ReadString();
        packetBuffer.ReadVarInt();
        
        MinecraftNetworkingClient.Instance!.Connection!.UpdateConnectionState(ConnectionState.Play);
    }
}