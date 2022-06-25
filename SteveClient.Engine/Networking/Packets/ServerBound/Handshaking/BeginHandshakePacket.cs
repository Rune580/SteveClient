using SteveClient.Engine.Networking.Connections;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Handshaking;

public class BeginHandshakePacket : ServerBoundPacket
{
    public int ProtocolVersion { get; set; }
    public string Address { get; set; }
    public ushort Port { get; set; }
    public ConnectionState NextState { get; set; }

    public BeginHandshakePacket(string address, ushort port, ConnectionState nextState)
    {
        ProtocolVersion = ProtocolDefinition.ProtocolVersion;
        Address = address;
        Port = port;
        NextState = nextState;
    }

    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteVarInt(ProtocolVersion);
        packetBuffer.WriteString(Address);
        packetBuffer.WriteUnsignedShort(Port);
        packetBuffer.WriteEnum(NextState);
    }
}