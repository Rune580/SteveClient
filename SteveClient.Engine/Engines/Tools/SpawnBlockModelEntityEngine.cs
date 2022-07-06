using System.Collections.Concurrent;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors.World;
using SteveClient.Engine.ECS;
using SteveClient.Minecraft.Data;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Tools;

public class SpawnBlockModelEntityEngine : BaseEngine
{
    private readonly IEntityFactory _entityFactory;

    public SpawnBlockModelEntityEngine(IEntityFactory entityFactory)
    {
        _entityFactory = entityFactory;
    }

    public override void Ready()
    {
        base.Ready();

        EntityInitializer initializer =
            _entityFactory.BuildEntity<WorldBlockStateDescriptor>(Egid.NextId, GameGroups.WorldBlocks.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new MinecraftBlockStateComponent(0));
    }

    public override void Execute(float delta)
    {
        if (BlockStateQueue.IsEmpty)
            return;

        if (!BlockStateQueue.TryDequeue(out var blockStateId))
            return;

        var optional = entitiesDB.QueryUniqueEntityOptional<MinecraftBlockStateComponent>(GameGroups.WorldBlocks.BuildGroup);
        ref var component = ref optional.Get1();

        component.BlockStateId = blockStateId;
    }

    private static readonly ConcurrentQueue<int> BlockStateQueue = new();

    public static void LoadBlockState(string resourceName)
    {
        BlockStateQueue.Enqueue(Blocks.GetDefaultBlockStateId(resourceName));
    }

    public static void LoadBlockState(int blockStateId)
    {
        BlockStateQueue.Enqueue(blockStateId);
    }
}