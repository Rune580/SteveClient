using SteveClient.Minecraft.Data.Schema.BlockStates;

namespace SteveClient.Minecraft.Data;

public static class BlockStateModels
{
    private static readonly Dictionary<string, BlockStateJson> BlockStateJsons = new();

    internal static void Add(string resourceName, BlockStateJson blockStateJson)
    {
        BlockStateJsons[resourceName] = blockStateJson;
    }

    public static BlockStateJson Get(string resourceName)
    {
        return BlockStateJsons[resourceName];
    }
}