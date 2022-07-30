using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing.EntityManipulation;

public class SpawnPlayerEntityEngine : PacketProcessingEngine<JoinGamePacket>
{
    private readonly IEntityFactory _entityFactory;

    public SpawnPlayerEntityEngine(IEntityFactory entityFactory)
    {
        _entityFactory = entityFactory;
    }

    protected override void Execute(float delta, ConsumablePacket<JoinGamePacket> consumablePacket)
    {
        JoinGamePacket packet = consumablePacket.Get();
        
        SpawnPlayerEntity(packet);
        
        consumablePacket.MarkConsumed();
    }

    private void SpawnPlayerEntity(JoinGamePacket joinGamePacket)
    {
        EntityInitializer initializer =
            _entityFactory.BuildEntity<PlayerDescriptor>(Egid.NextId, GameGroups.Player.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new RigidBodyComponent());
        initializer.Init(new MinecraftEntityComponent(joinGamePacket.EntityId));
        initializer.Init(new HeadComponent());
        initializer.Init(new ModelFilterComponent(Primitives.Cube));
        initializer.Init(new PlayerComponent());
    }
}