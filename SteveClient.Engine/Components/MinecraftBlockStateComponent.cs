using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct MinecraftBlockStateComponent : IEntityComponent
{
    public int BlockStateId;

    public MinecraftBlockStateComponent(int blockStateId)
    {
        BlockStateId = blockStateId;
    }
}

public static class MinecraftBlockStateComponentExtensions
{
    public static VoxelShape GetCollisionVoxelShape(this MinecraftBlockStateComponent component)
    {
        return Blocks.GetBlockStateFromBlockStateId(component.BlockStateId).CollisionShape;
    }
}