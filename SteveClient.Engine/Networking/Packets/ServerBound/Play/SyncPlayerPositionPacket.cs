using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Play;

public class SyncPlayerPositionPacket : ServerBoundPacket
{
    private readonly Vector3d _playerPos;
    private readonly bool _onGround;

    public SyncPlayerPositionPacket(Vector3d playerPos, bool onGround)
    {
        _playerPos = playerPos;
        _onGround = onGround;
    }

    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteVector3d(_playerPos);
        packetBuffer.WriteBool(_onGround);
    }
}