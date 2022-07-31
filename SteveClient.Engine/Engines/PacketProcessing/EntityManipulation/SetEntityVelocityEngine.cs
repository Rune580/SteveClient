using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class SetEntityVelocityEngine : PacketProcessingEngine<SetEntityVelocityPacket>
{
    private readonly World _world;
    
    public SetEntityVelocityEngine(World world)
    {
        _world = world;
    }
    
    protected override void Execute(float delta, ConsumablePacket<SetEntityVelocityPacket> consumablePacket)
    {
        SetEntityVelocityPacket packet = consumablePacket.Get();
        
        if (!_world.MinecraftEntityIdMap.ContainsKey(packet.EntityId))
        {
            consumablePacket.MarkConsumed();
            return;
        }

        foreach (var ((rigidBodies, entities, count), _) in entitiesDB
                     .QueryEntities<RigidBodyComponent, MinecraftEntityComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var entity = ref entities[i];

                if (entity.EntityId != packet.EntityId)
                    continue;

                ref var rigidBody = ref rigidBodies[i];

                rigidBody.Velocity = packet.Velocity;
                
                consumablePacket.MarkConsumed();
                break;
            }

            if (consumablePacket.Consumed)
                break;
        }
    }
}