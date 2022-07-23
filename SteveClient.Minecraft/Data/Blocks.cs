using System.Runtime.InteropServices;
using SteveClient.Minecraft.BlockStructs;

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

    private static int _maxBitsPerEntry;

    public static int MaxBitsPerEntry
    {
        get
        {
            if (_maxBitsPerEntry > 0)
                return _maxBitsPerEntry;

            _maxBitsPerEntry = (int)Math.Ceiling(Math.Log2(Instance._blockStates.Count));
            
            return _maxBitsPerEntry;
        }
    }

    public static Block GetBlock(int id)
    {
        return Instance._blocks[id];
    }

    public static Block[] GetBlocks()
    {
        return Instance._blocks.Values.ToArray();
    }

    public static BlockState GetBlockState(int blockStateId)
    {
        return Instance._blockStates[blockStateId];
    }

    public static BlockState[] GetBlockStates()
    {
        return Instance._blockStates.Values.ToArray();
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