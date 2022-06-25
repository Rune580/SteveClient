namespace SteveClient.Minecraft.Blocks;

public readonly struct Block
{
    public readonly int Id;
    public readonly string ResourceName;
    public readonly float ExplosionResistance;
    public readonly float Friction;
    public readonly float SpeedFactor;
    public readonly float JumpFactor;
    public readonly int DefaultStateId;
    public readonly Dictionary<int, BlockState> BlockStates;

    public Block(int id, string resourceName, float explosionResistance, float friction, float speedFactor, float jumpFactor, int defaultStateId, Dictionary<int, BlockState> blockStates)
    {
        Id = id;
        ResourceName = resourceName;
        ExplosionResistance = explosionResistance;
        Friction = friction;
        SpeedFactor = speedFactor;
        JumpFactor = jumpFactor;
        DefaultStateId = defaultStateId;
        BlockStates = blockStates;
    }
}

public static class BlockExtensions
{
    public static ref readonly BlockState GetDefaultBlockState(this in Block block)
    {
        return ref Data.Blocks.GetBlockStateFromBlockStateId(block.DefaultStateId);
    }
}