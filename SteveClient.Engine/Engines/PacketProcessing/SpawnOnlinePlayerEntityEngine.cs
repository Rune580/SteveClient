using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing;

public class SpawnOnlinePlayerEntityEngine : PacketProcessingEngine<SpawnPlayerPacket>
{
    private readonly IEntityFactory _entityFactory;
    private readonly World _world;

    public SpawnOnlinePlayerEntityEngine(IEntityFactory entityFactory, World world)
    {
        _entityFactory = entityFactory;
        _world = world;
    }

    protected override void Execute(float delta, ConsumablePacket<SpawnPlayerPacket> consumablePacket)
    {
        SpawnPlayerPacket packet = consumablePacket.Get();
        
        SpawnOnlinePlayerEntity(packet);
        
        consumablePacket.MarkConsumed();
    }

    private void SpawnOnlinePlayerEntity(SpawnPlayerPacket packet)
    {
        uint id = Egid.NextId;

        EntityInitializer initializer =
            _entityFactory.BuildEntity<MinecraftEntityDescriptor>(id, GameGroups.MinecraftEntities.BuildGroup);
        
        initializer.Init(new TransformComponent((Vector3)packet.Position));
        initializer.Init(new MinecraftEntityComponent(packet.EntityId));
        initializer.Init(new ModelFilterComponent(Primitives.Cube));

        _world.MinecraftEntityIdMap[packet.EntityId] = id;
    }
}