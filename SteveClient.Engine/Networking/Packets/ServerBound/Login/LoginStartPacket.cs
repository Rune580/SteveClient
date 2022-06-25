using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Login;

public class LoginStartPacket : ServerBoundPacket
{
    public String Name { get; set; }
    public bool HasSigData { get; set; }

    public LoginStartPacket(string name)
    {
        Name = name;
        HasSigData = false;
    }
    
    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteString(Name);
        packetBuffer.WriteBool(HasSigData);
    }
}