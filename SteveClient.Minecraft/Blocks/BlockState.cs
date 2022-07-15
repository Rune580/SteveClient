using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Blocks;

public readonly struct BlockState
{
    public readonly int BlockId;
    public readonly int StateId;
    public readonly int Opacity;
    public readonly bool Occludes;
    public readonly int CollisionVoxelShapeId;
    public readonly int OcclusionVoxelShapeId;
    
    public BlockState(int blockId, int stateId, int opacity, bool occludes, string collisionShapeData, string occlusionShapeData)
    {
        BlockId = blockId;
        StateId = stateId;
        Opacity = opacity;
        Occludes = occludes;

        CollisionVoxelShapeId = VoxelShapes.Add(collisionShapeData);
        OcclusionVoxelShapeId = VoxelShapes.Add(occlusionShapeData);
    }
    
    public VoxelShape CollisionShape => VoxelShapes.Get(CollisionVoxelShapeId);
    public VoxelShape OcclusionShape => VoxelShapes.Get(OcclusionVoxelShapeId);
}