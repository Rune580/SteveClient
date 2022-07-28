using SteveClient.Engine.Rendering.Builders;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.Models.BlockModelVariants;
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
                var variantJson = variants.Get(blockState.BlockProperties);

                var variantBlockModel = BuildVariantModel(builder, blockModels, variantJson);

                ModelRegistry.BlockStateModels[blockStateId] = variantBlockModel;
            }
        }
        
        BlockModelLoaders.Clear();
    }

    private static VariantBlockModel BuildVariantModel(in BlockModelBuilder builder, in Dictionary<string, RawBlockModel> blockModels, in VariantModelJson[] variantModels)
    {
        int count = variantModels.Length;

        VariantBlockModel variantBlockModel = new VariantBlockModel(count);

        for (int i = 0; i < count; i++)
        {
            var variantJson = variantModels[i];
            
            string modelPath = variantJson.Model
                .Replace("minecraft:", "")
                .Replace("block/", "");
            RawBlockModel rawModel = blockModels[modelPath];

            builder.AddRawBlockModel(rawModel, variantJson);
            var model = builder.Build();

            int weight = variantJson.Weight ?? 1;
            
            variantBlockModel.Add(model, weight);
        }

        return variantBlockModel;
    }
}