using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.Models.BlockModelVariants;
using SteveClient.Minecraft.BlockStructs;
using SteveClient.Minecraft.Data;

namespace SteveClient.Engine.AssetManagement;

public static class ModelRegistry
{
    public static readonly List<SimpleInternalModel> SimpleInternalModels = new();
    public static readonly List<InternalMesh> InternalMeshes = new();

    public static readonly Dictionary<int, IBlockModel> BlockStateModels = new();

    public static bool TryGetBlockModel(this ref BlockState blockState, out IBlockModel model)
    {
        model = null!;

        if (!BlockStateModels.ContainsKey(blockState.StateId))
            return false;

        model = BlockStateModels[blockState.StateId];
        return true;
    }
}