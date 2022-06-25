namespace SteveClient.Minecraft.Blocks;

public readonly struct BlockState
{
    public readonly int BlockId;
    public readonly int StateId;
    

    public BlockState(int blockId, int stateId)
    {
        BlockId = blockId;
        StateId = stateId;
    }
}