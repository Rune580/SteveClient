using SteveClient.Engine.Rendering.Builders;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.BlockStructs;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Data.Schema.BlockStates;
using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Engine.AssetManagement.ModelLoading;

public static class BlockModelLoader
{
    public static void LoadBlockModels()
    {
        Dictionary<string, RawBlockModel> blockModels = BlockModelLoaders.GetBlockModels();
        Block[] blocks = Blocks.GetBlocks();
        
        BlockModelBuilder builder = new BlockModelBuilder();
        
        foreach (var block in blocks)
        {
            var resourceName = block.ResourceName.Replace("minecraft:", "");
            var variants = BlockStateModels.Get(resourceName).Variants;

            if (variants is null)
                continue; // Todo: multipart

            foreach (var (blockStateId, blockState) in block.BlockStates)
            {
                var variantJson = variants.Get(blockState.BlockProperties)[0];

                string modelPath = variantJson.Model
                    .Replace("minecraft:", "")
                    .Replace("block/", "");
                RawBlockModel model = blockModels[modelPath];

                builder.AddRawBlockModel(model, variantJson);

                ModelRegistry.BlockStateModels[blockStateId] = builder.Build();
            }
        }
        
        BlockModelLoaders.Clear();
    }
}