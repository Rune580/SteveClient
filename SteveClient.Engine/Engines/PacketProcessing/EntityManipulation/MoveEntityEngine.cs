using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class MoveEntityEngine : PacketProcessingEngine<EntityPositionPacket>
{
    private readonly World _world;

    public MoveEntityEngine(World world)
    {
        _world = world;
    }
    
    protected override void Execute(float delta, ConsumablePacket<EntityPositionPacket> consumablePacket)
    {
        EntityPositionPacket packet = consumablePacket.Get();
        
        if (!_world.MinecraftEntityIdMap.ContainsKey(packet.EntityId))
        {
            consumablePacket.MarkConsumed();
            return;
        }
        
        foreach (var ((transforms, entities, count), _) in entitiesDB
                     .QueryEntities<TransformComponent, MinecraftEntityComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var entity = ref entities[i];

                if (entity.EntityId != packet.EntityId)
                    continue;

                transform.Position = ApplyDelta(transform.Position, packet.Delta);
                
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
}