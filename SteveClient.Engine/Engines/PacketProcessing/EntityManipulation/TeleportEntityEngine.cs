using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class TeleportEntityEngine : PacketProcessingEngine<TeleportEntityPacket>
{
    private readonly World _world;
    
    public TeleportEntityEngine(World world)
    {
        _world = world;
    }

    protected override void Execute(float delta, ConsumablePacket<TeleportEntityPacket> consumablePacket)
    {
        TeleportEntityPacket packet = consumablePacket.Get();

        if (!_world.MinecraftEntityIdMap.ContainsKey(packet.EntityId))
        {
            consumablePacket.MarkConsumed();
            return;
        }
        
        foreach (var ((transforms, entities, count), _) in entitiesDB.QueryEntities<TransformComponent, MinecraftEntityComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var entity = ref entities[i];

                if (entity.EntityId != packet.EntityId)
                    continue;
                
                transform.Position = (Vector3)packet.Position;
                
                consumablePacket.MarkConsumed();
                break;
            }

            if (consumablePacket.Consumed)
                break;
        }
    }
}