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

    public static Block GetBlockFromId(int id)
    {
        return Instance._blocks[id];
    }

    public static BlockState GetBlockState(int blockStateId)
    {
        return Instance._blockStates[blockStateId];
    }

    public static ref readonly BlockState GetDefaultBlockState(string blockResourceName)
    {
        return ref CollectionsMarshal.GetValueRefOrNullRef(Instance._blockStates, GetDefaultBlockStateId(blockResourceName));
    }

    public static int GetDefaultBlockStateId(string blockResourceName)
    {
        return Instance._resourceNameBlockStateMap[blockResourceName];
    }

    public static string GetResourceName(int blockStateId)
    {
        return Instance._blockStateResourceNameMap[blockStateId];
    }

    private readonly Dictionary<int, Block> _blocks = new();
    private readonly Dictionary<int, BlockState> _blockStates = new();
    private readonly Dictionary<string, int> _resourceNameBlockStateMap = new();
    private readonly Dictionary<int, string> _blockStateResourceNameMap = new();

    private Blocks(Block[] blocks)
    {
        if (_instance is not null)
            throw new Exception("Instance of Blocks already exists!");

        foreach (var block in blocks)
        {
            _blocks[block.Id] = block;

            foreach (var (blockStateId, blockState) in block.BlockStates)
            {
                _blockStates[blockStateId] = blockState;
                _blockStateResourceNameMap[blockStateId] = block.ResourceName;
            }

            _resourceNameBlockStateMap[block.ResourceName.Replace("minecraft:", "")] = block.DefaultStateId;
        }
        
        _instance = this;
    }
}