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
        var optional = entitiesDB.QueryUniqueEntityOptional<TransformComponent, HeadComponent>(GameGroups.Player.BuildGroup);

        if (!optional.HasValue)
            return;

        ref var transform = ref optional.Get1();
        ref var head = ref optional.Get2();

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

        float yaw = -(packet.Yaw * (MathF.PI / 180f));
        if ((packet.Flags & RelativeFlags.XRot) != 0)
        {
            var currentYaw = transform.Rotation.ToEulerAngles().Y * (MathF.PI) / 180f;
            transform.Rotation = Quaternion.FromEulerAngles(0, currentYaw + yaw, 0);
        }
        else
        {
            transform.Rotation = Quaternion.FromEulerAngles(0, yaw, 0);
        }
        
        float pitch = packet.Pitch * (MathF.PI / 180f);
        if ((packet.Flags & RelativeFlags.YRot) != 0)
        {
            head.Pitch += pitch;
        }
        else
        {
            head.Pitch = pitch;
        }
        
        consumablePacket.MarkConsumed();
    }
}