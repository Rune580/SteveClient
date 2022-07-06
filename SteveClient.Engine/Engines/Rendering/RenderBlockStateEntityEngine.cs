using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Components;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils;
using SteveClient.Minecraft.Data;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Rendering;

public class RenderBlockStateEntityEngine : RenderingEngine
{
    private readonly BlockRenderHelper _renderHelper;

    public RenderBlockStateEntityEngine()
    {
        _renderHelper = new BlockRenderHelper();
    }
    
    public override void Execute(float delta)
    {
        foreach (var ((transforms, blockStates, count), _) in entitiesDB.QueryEntities<TransformComponent, MinecraftBlockStateComponent>(GameGroups.WorldBlocks.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var blockStateComp = ref blockStates[i];

                string resourceName = Blocks.GetResourceName(blockStateComp.BlockStateId).Replace("minecraft:", "");

                if (!ModelRegistry.BlockModels.ContainsKey(resourceName))
                    continue;

                _renderHelper.WithBlockModel(ModelRegistry.BlockModels[resourceName])
                    .Translate(BlockPosMenu.Position)
                    .WithColor(Color4.White)
                    .Upload(RenderLayerDefinitions.ScreenSpacePositionTextureLayer);
            }
        }
    }
}