using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound;

public abstract class ServerBoundPacket : IServerBoundPacket
{
    public abstract void Write(in OutPacketBuffer packetBuffer);

    public void SendToServer() => ((IServerBoundPacket)this).Send();
}