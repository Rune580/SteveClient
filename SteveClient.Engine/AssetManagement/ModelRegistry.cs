using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.BlockStructs;
using SteveClient.Minecraft.Data;

namespace SteveClient.Engine.AssetManagement;

public static class ModelRegistry
{
    public static readonly List<SimpleInternalModel> SimpleInternalModels = new();
    public static readonly List<InternalMesh> InternalMeshes = new();

    public static readonly Dictionary<int, BlockModel> BlockStateModels = new();

    public static bool TryGetBlockModel(this ref BlockState blockState, out BlockModel model)
    {
        model = default;

        if (!BlockStateModels.ContainsKey(blockState.StateId))
            return false;

        model = BlockStateModels[blockState.StateId];
        return true;
    }
}