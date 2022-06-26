using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;

namespace SteveClient.Engine.Engines.PacketProcessing;

public class TeleportPlayerEntityEngine : PacketProcessingEngine<PlayerPositionAndLookPacket>
{
    protected override void Execute(float delta, ConsumablePacket<PlayerPositionAndLookPacket> consumablePacket)
    {
        var packet = consumablePacket.Get();
        var optionalTransform = entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.Player.BuildGroup);

        if (!optionalTransform.HasValue)
            return;

        ref var transform = ref optionalTransform.Get1();

        transform.Position = (Vector3)packet.Position;
        
        Quaternion yaw = Quaternion.FromAxisAngle(Vector3.UnitZ, packet.Yaw);
        Quaternion pitch = Quaternion.FromAxisAngle(Vector3.UnitY, packet.Pitch);

        transform.Rotation = yaw * pitch;
        
        consumablePacket.MarkConsumed();
    }
}