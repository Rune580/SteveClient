using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Blocks;

public readonly struct BlockState
{
    public readonly int BlockId;
    public readonly int StateId;
    public readonly int CollisionVoxelShapeId;
    public readonly int Opacity;
    public readonly bool Occludes;
    
    public BlockState(int blockId, int stateId, string collisionShapeData, int opacity, bool occludes)
    {
        BlockId = blockId;
        StateId = stateId;
        Opacity = opacity;
        Occludes = occludes;
        CollisionVoxelShapeId = VoxelShapes.Add(collisionShapeData);
    }
    
    public VoxelShape CollisionShape => VoxelShapes.Get(CollisionVoxelShapeId);
}