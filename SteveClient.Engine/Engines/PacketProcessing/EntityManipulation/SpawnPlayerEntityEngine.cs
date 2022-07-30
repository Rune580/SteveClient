using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class SpawnPlayerEntityEngine : PacketProcessingEngine<JoinGamePacket>
{
    private readonly IEntityFactory _entityFactory;
    private readonly World _world;

    public SpawnPlayerEntityEngine(IEntityFactory entityFactory, World world)
    {
        _entityFactory = entityFactory;
        _world = world;
    }

    protected override void Execute(float delta, ConsumablePacket<JoinGamePacket> consumablePacket)
    {
        JoinGamePacket packet = consumablePacket.Get();
        
        SpawnPlayerEntity(packet);

        consumablePacket.MarkConsumed();
    }

    private void SpawnPlayerEntity(JoinGamePacket joinGamePacket)
    {
        uint id = Egid.NextId;

        _world.MinecraftEntityIdMap[joinGamePacket.EntityId] = id;
        
        EntityInitializer initializer =
            _entityFactory.BuildEntity<PlayerDescriptor>(id, GameGroups.Player.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new RigidBodyComponent());
        initializer.Init(new MinecraftEntityComponent(joinGamePacket.EntityId));
        initializer.Init(new HeadComponent());
        initializer.Init(new ModelFilterComponent(Primitives.Cube));
        initializer.Init(new PlayerComponent());
    }
}