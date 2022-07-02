using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Minecraft.Data;

public static class BlockModels
{
    private static readonly Dictionary<string, BlockModel> Models = new();

    public static void Add(string resourceName, BlockModel model)
    {
        Models[resourceName] = model;
    }

    public static BlockModel GetBlockModel(string resourceName)
    {
        return Models[resourceName];
    }

    public static BlockModel[] GetBlockModels()
    {
        return Models.Values.ToArray();
    }

    public static void Clear()
    {
        Models.Clear();
    }
}