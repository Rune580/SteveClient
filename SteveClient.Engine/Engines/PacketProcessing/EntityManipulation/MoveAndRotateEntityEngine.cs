using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class MoveAndRotateEntityEngine : PacketProcessingEngine<EntityPositionAndRotationPacket>
{
    private readonly World _world;
    
    public MoveAndRotateEntityEngine(World world)
    {
        _world = world;
    }

    protected override void Execute(float delta, ConsumablePacket<EntityPositionAndRotationPacket> consumablePacket)
    {
        EntityPositionAndRotationPacket packet = consumablePacket.Get();

        if (!_world.MinecraftEntityIdMap.ContainsKey(packet.EntityId))
        {
            consumablePacket.MarkConsumed();
            return;
        }
        
        foreach (var ((transforms, heads, entities, count), _) in entitiesDB.QueryEntities<TransformComponent, HeadComponent, MinecraftEntityComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var entity = ref entities[i];

                if (entity.EntityId != packet.EntityId)
                    continue;
                
                ref var transform = ref transforms[i];
                ref var head = ref heads[i];
                
                transform.Position = ApplyDelta(transform.Position, packet.Delta);
                transform.Rotation = Quaternion.FromEulerAngles(0, -AngleToRadians(packet.Yaw), 0);

                head.Pitch = AngleToRadians(packet.Pitch);
                
                consumablePacket.MarkConsumed();
                break;
            }

            if (consumablePacket.Consumed)
                break;
        }
    }
    
    private static Vector3 ApplyDelta(Vector3 position, Vector3i delta)
    {
        Vector3i current = (Vector3i)(position * 32 * 128);

        current += delta;

        return new Vector3(current.X / 128f / 32f, current.Y / 128f / 32f, current.Z / 128f / 32f);
    }

    private static float AngleToRadians(byte angle)
    {
        float angleToDegrees = (1 / 256f) * 360;

        float degrees = angle * angleToDegrees;

        return degrees * (MathF.PI / 180f);
    }
}