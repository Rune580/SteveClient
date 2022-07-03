using SteveClient.Engine.Rendering.Models;

namespace SteveClient.Engine.AssetManagement;

public static class ModelRegistry
{
    public static readonly List<SimpleModel> Models = new();

    public static readonly Dictionary<string, BlockModel> BlockModels = new();
}