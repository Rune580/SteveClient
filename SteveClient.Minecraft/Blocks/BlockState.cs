using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Blocks;

public readonly struct BlockState
{
    public readonly int BlockId;
    public readonly int StateId;
    public readonly int CollisionVoxelShapeId;
    
    public BlockState(int blockId, int stateId, string collisionShapeData)
    {
        BlockId = blockId;
        StateId = stateId;
        CollisionVoxelShapeId = VoxelShapes.Add(collisionShapeData);
    }
    
    public VoxelShape CollisionShape => VoxelShapes.Get(CollisionVoxelShapeId);
}