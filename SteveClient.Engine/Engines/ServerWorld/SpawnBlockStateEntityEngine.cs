using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors.World;
using SteveClient.Engine.Game;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Numerics;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.ServerWorld;

public class SpawnBlockStateEntityEngine : BaseEngine
{
    private readonly IEntityFactory _entityFactory;
    private readonly World _world;
    
    public SpawnBlockStateEntityEngine(IEntityFactory entityFactory, World world)
    {
        _entityFactory = entityFactory;
        _world = world;
    }

    public override void Execute(float delta)
    {
        
    }
    
    private void GenerateBlocksInSection(ref ChunkSectionComponent sectionComponent)
    {
        
    }

    private void SpawnBlockStateEntity(int blockStateId, Vector3i position)
    {
        EntityInitializer initializer =
            _entityFactory.BuildEntity<WorldBlockStateDescriptor>(Egid.NextId, GameGroups.ChunkSections.BuildGroup);
        
        initializer.Init(new TransformComponent(position));
        initializer.Init(new MinecraftBlockStateComponent(blockStateId));
    }
}