using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Engines.Rendering;
using SteveClient.Engine.Rendering.Builders;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
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

                var renderer = new RenderBuilder();

                renderer.WithMesh(ModelRegistry.Models[modelFilter.ModelIndex])
                    .WithColor(Color4.White)
                    .WithTransform(ref transform)
                    .Upload(RenderLayerDefinitions.PositionColorRenderLayer);
            }
        }
        
        RenderLayerDefinitions.PositionColorRenderLayer.RebuildBuffers();
    }
}