using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Data;

namespace SteveClient.Engine.AssetManagement;

public static class ModelRegistry
{
    public static readonly List<SimpleInternalModel> SimpleInternalModels = new();
    public static readonly List<InternalMesh> InternalMeshes = new();

    public static readonly Dictionary<string, BlockModel> BlockModels = new();

    public static bool TryGetBlockModel(this ref BlockState blockState, out BlockModel model)
    {
        model = default;
        
        string resourceName = Blocks.GetResourceName(blockState.StateId).Replace("minecraft:", "");

        if (!BlockModels.ContainsKey(resourceName))
            return false;

        model = BlockModels[resourceName];

        return true;
    }
}