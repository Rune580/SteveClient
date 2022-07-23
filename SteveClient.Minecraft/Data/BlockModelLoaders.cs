using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Minecraft.Data;

public static class BlockModelLoaders
{
    private static readonly Dictionary<string, RawBlockModel> Models = new();

    public static void Add(string resourceName, RawBlockModel model)
    {
        Models[resourceName] = model;
    }

    public static RawBlockModel GetBlockModel(string resourceName)
    {
        return Models[resourceName];
    }

    public static RawBlockModel[] GetBlockModelsArray()
    {
        return Models.Values.ToArray();
    }

    public static Dictionary<string, RawBlockModel> GetBlockModels()
    {
        return Models;
    }

    public static void Clear()
    {
        Models.Clear();
    }
}