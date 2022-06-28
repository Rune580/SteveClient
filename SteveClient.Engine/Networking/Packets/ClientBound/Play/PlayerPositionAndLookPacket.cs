using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Packets.ServerBound.Play;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class PlayerPositionAndLookPacket : ClientBoundPacket
{
    public Vector3d Position;
    public float Yaw;
    public float Pitch;
    public RelativeFlags Flags;
    public int TeleportId;
    public bool DismountVehicle;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Position = packetBuffer.ReadVector3d();
        Yaw = packetBuffer.ReadFloat();
        Pitch = packetBuffer.ReadFloat();
        Flags = (RelativeFlags)packetBuffer.ReadUnsignedByte();
        TeleportId = packetBuffer.ReadVarInt();
        DismountVehicle = packetBuffer.ReadBool();
        
        new TeleportConfirmPacket(TeleportId).SendToServer();
    }

    [Flags]
    public enum RelativeFlags
    {
        X = 1,
        Y = 2,
        Z = 4,
        YRot = 8,
        XRot = 16
    }
}