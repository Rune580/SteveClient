using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class RotateEntityEngine : PacketProcessingEngine<EntityRotationPacket>
{
    private readonly World _world;
    
    public RotateEntityEngine(World world)
    {
        _world = world;
    }
    
    protected override void Execute(float delta, ConsumablePacket<EntityRotationPacket> consumablePacket)
    {
        EntityRotationPacket packet = consumablePacket.Get();

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
                
                transform.Rotation = Quaternion.FromEulerAngles(0, -AngleToRadians(packet.Yaw), 0);

                head.Pitch = AngleToRadians(packet.Pitch);
                
                consumablePacket.MarkConsumed();
                break;
            }

            if (consumablePacket.Consumed)
                break;
        }
    }
    
    private static float AngleToRadians(byte angle)
    {
        float angleToDegrees = (1 / 256f) * 360;

        float degrees = angle * angleToDegrees;

        return degrees * (MathF.PI / 180f);
    }
}