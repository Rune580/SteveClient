using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Data.Structs;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.BlockStructs;

public readonly struct BlockState
{
    public readonly int BlockId;
    public readonly int StateId;
    public readonly int Opacity;
    public readonly bool Occludes;
    public readonly bool Air;
    public readonly bool Liquid;
    public readonly int CollisionVoxelShapeId;
    public readonly int OcclusionVoxelShapeId;
    public readonly BlockProperties BlockProperties;
    
    public BlockState(int blockId,
        int stateId,
        int opacity,
        bool occludes,
        bool air,
        bool liquid,
        string collisionShapeData,
        string occlusionShapeData,
        BlockProperties blockProperties)
    {
        BlockId = blockId;
        StateId = stateId;
        Opacity = opacity;
        Occludes = occludes;
        Air = air;
        Liquid = liquid;
        BlockProperties = blockProperties;

        CollisionVoxelShapeId = VoxelShapes.Add(collisionShapeData);
        OcclusionVoxelShapeId = VoxelShapes.Add(occlusionShapeData);
    }
    
    public VoxelShape CollisionShape => VoxelShapes.Get(CollisionVoxelShapeId);
    public VoxelShape OcclusionShape => VoxelShapes.Get(OcclusionVoxelShapeId);
}