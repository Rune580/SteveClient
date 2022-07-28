using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using static SteveClient.Engine.Networking.Packets.ClientBound.Play.PlayerPositionAndLookPacket;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class TeleportPlayerEntityEngine : PacketProcessingEngine<PlayerPositionAndLookPacket>
{
    protected override void Execute(float delta, ConsumablePacket<PlayerPositionAndLookPacket> consumablePacket)
    {
        var packet = consumablePacket.Get();
        var optionalTransform = entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.Player.BuildGroup);

        if (!optionalTransform.HasValue)
            return;

        ref var transform = ref optionalTransform.Get1();

        Vector3 targetPosition = transform.Position;

        if ((packet.Flags & RelativeFlags.X) != 0)
        {
            targetPosition.X += (float)packet.Position.X;
        }
        else
        {
            targetPosition.X = (float)packet.Position.X;
        }

        if ((packet.Flags & RelativeFlags.Y) != 0)
        {
            targetPosition.Y += (float)packet.Position.Y;
        }
        else
        {
            targetPosition.Y = (float)packet.Position.Y;
        }

        if ((packet.Flags & RelativeFlags.Z) != 0)
        {
            targetPosition.Z += (float)packet.Position.Z;
        }
        else
        {
            targetPosition.Z = (float)packet.Position.Z;
        }

        transform.Position = targetPosition;

        Quaternion yaw = Quaternion.FromAxisAngle(Vector3.UnitZ, packet.Yaw);
        Quaternion pitch = Quaternion.FromAxisAngle(Vector3.UnitY, packet.Pitch);

        // transform.Rotation = yaw * pitch;
        
        consumablePacket.MarkConsumed();
    }
}