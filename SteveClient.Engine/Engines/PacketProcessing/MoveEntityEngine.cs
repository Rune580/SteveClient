﻿using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing;

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

        foreach (var ((transforms, entities, count), _) in entitiesDB.QueryEntities<TransformComponent, MinecraftEntityComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var entity = ref entities[i];

                if (entity.EntityId != packet.EntityId)
                    continue;

                Vector3 targetPos = (Vector3)(transform.Position + packet.Delta);
                transform.Position = targetPos;
                
                consumablePacket.MarkConsumed();
                break;
            }

            if (consumablePacket.Consumed)
                break;
        }

        if (!_world.MinecraftEntityIdMap.ContainsKey(packet.EntityId))
            consumablePacket.MarkConsumed();
    }
}