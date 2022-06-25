using System.Runtime.InteropServices;
using SteveClient.Minecraft.Blocks;

namespace SteveClient.Minecraft.Data;

public class Blocks
{
    private static Blocks? _instance;

    internal static Blocks Instance
    {
        get
        {
            if (_instance is null)
                throw new NullReferenceException();

            return _instance;
        }
    }

    internal static void LoadFromArray(Block[] blocks)
    {
        new Blocks(blocks);
    }

    public static ref readonly Block GetBlockFromId(int id)
    {
        return ref CollectionsMarshal.GetValueRefOrNullRef(Instance._blocks, id); // This might be a terrible idea
    }

    public static ref readonly BlockState GetBlockStateFromBlockStateId(int blockStateId)
    {
        return ref CollectionsMarshal.GetValueRefOrNullRef(Instance._blockStates, blockStateId);
    }

    private readonly Dictionary<int, Block> _blocks = new();
    private readonly Dictionary<int, BlockState> _blockStates = new();

    private Blocks(Block[] blocks)
    {
        if (_instance is not null)
            throw new Exception("Instance of Blocks already exists!");

        foreach (var block in blocks)
        {
            _blocks[block.Id] = block;

            foreach (var (blockStateId, blockState) in block.BlockStates)
                _blockStates[blockStateId] = blockState;
        }
        
        _instance = this;
    }
}