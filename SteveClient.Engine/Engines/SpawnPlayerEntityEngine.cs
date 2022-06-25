using System.Collections.Concurrent;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class SpawnPlayerEntityEngine : IQueryingEntitiesEngine, IScheduledEngine
{
    public static readonly ConcurrentQueue<JoinGamePacket> JoinGamePackets = new();
    
    private readonly IEntityFactory _entityFactory;
    
    public EntitiesDB entitiesDB { get; set; }
    public void Ready() { }

    public SpawnPlayerEntityEngine(IEntityFactory entityFactory)
    {
        _entityFactory = entityFactory;
    }
    
    public void Execute(float delta)
    {
        if (JoinGamePackets.Count <= 0)
            return;
        
        if (!JoinGamePackets.TryDequeue(out var joinGamePacket))
            return;
        
        SpawnPlayerEntity(joinGamePacket);
    }

    private void SpawnPlayerEntity(JoinGamePacket joinGamePacket)
    {
        EntityInitializer initializer =
            _entityFactory.BuildEntity<PlayerDescriptor>(Egid.NextId, GameGroups.PlayerEntities.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new MinecraftEntityComponent(joinGamePacket.EntityId));
        initializer.Init(new ModelFilterComponent(Primitives.Cube));
        initializer.Init(new PlayerComponent());
    }
}