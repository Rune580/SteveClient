using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Minecraft.Data;

public static class BlockModels
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

    public static RawBlockModel[] GetBlockModels()
    {
        return Models.Values.ToArray();
    }

    public static void Clear()
    {
        Models.Clear();
    }
}