using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Play;

public class ClientStatusPacket : ServerBoundPacket
{
    public readonly ClientStatusAction Action;

    public ClientStatusPacket(ClientStatusAction action)
    {
        Action = action;
    }
    
    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteEnum(Action);
    }
    
    public enum ClientStatusAction
    {
        PerformRespawn,
        RequestStats
    }
}