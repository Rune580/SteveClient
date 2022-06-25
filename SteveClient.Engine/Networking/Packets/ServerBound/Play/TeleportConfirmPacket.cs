using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Play;

public class TeleportConfirmPacket : ServerBoundPacket
{
    private readonly int _teleportId;

    public TeleportConfirmPacket(int teleportId)
    {
        _teleportId = teleportId;
    }
    
    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteVarInt(_teleportId);
    }
}