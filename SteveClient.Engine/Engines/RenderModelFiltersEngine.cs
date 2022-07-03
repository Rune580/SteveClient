using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering.Builders;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class RenderModelFiltersEngine : BaseEngine
{
    public override void Execute(float delta)
    {
        RenderLayerDefinitions.PositionColorRenderLayer.Flush();

        foreach (var ((transforms, modelFilters, count), _) in entitiesDB.QueryEntities<TransformComponent, ModelFilterComponent>(GameGroups.ModelFilters.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var modelFilter = ref modelFilters[i];

                var renderBuilder = new SimpleRenderHelper();

                renderBuilder.WithMesh(ModelRegistry.Models[modelFilter.ModelIndex])
                    .WithColor(Color4.White)
                    .WithTransform(ref transform)
                    .Upload(RenderLayerDefinitions.PositionColorRenderLayer);
            }
        }
        
        RenderLayerDefinitions.PositionColorRenderLayer.RebuildBuffers();
    }
}