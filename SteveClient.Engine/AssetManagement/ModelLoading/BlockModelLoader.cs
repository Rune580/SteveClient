using SteveClient.Engine.Rendering.Builders;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Engine.AssetManagement.ModelLoading;

public static class BlockModelLoader
{
    public static void LoadBlockModels()
    {
        RawBlockModel[] rawBlockModels = BlockModels.GetBlockModels();

        BlockModelBuilder builder = new BlockModelBuilder();

        foreach (var rawBlockModel in rawBlockModels)
        {
            BlockModel blockModel = builder.AddRawBlockModel(rawBlockModel).Build();

            ModelRegistry.BlockModels[rawBlockModel.ResourceName] = blockModel;
        }
        
        BlockModels.Clear();
    }
}