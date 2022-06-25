using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class TeleportPlayerEntityEngine : IQueryingEntitiesEngine, IScheduledEngine
{
    public static readonly ConcurrentQueue<PlayerPositionAndLookPacket> PositionAndLookPackets = new();

    public EntitiesDB entitiesDB { get; set; }
    public void Ready() { }

    public void Execute(float delta)
    {
        if (PositionAndLookPackets.Count <= 0)
            return;

        if (!PositionAndLookPackets.TryPeek(out var positionAndLookPacket))
            return;

        foreach (var ((transforms, count), _) in entitiesDB.QueryEntities<TransformComponent>(GameGroups.PlayerEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];

                transform.Position = (Vector3)positionAndLookPacket.Position;
                transform.Rotate(positionAndLookPacket.Yaw, positionAndLookPacket.Pitch);
            }
            
            PositionAndLookPackets.TryDequeue(out _);
        }
    }
}