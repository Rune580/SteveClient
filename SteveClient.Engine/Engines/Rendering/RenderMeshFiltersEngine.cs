using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Rendering;

public class RenderMeshFiltersEngine : RenderingEngine
{
    public override void Execute(float delta)
    {
        foreach (var ((transforms, meshFilters, count), _) in entitiesDB.QueryEntities<TransformComponent, MeshFilterComponent>(GameGroups.MeshFilters.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var meshFilter = ref meshFilters[i];

                var renderBuilder = new SimpleRenderHelper();

                renderBuilder.WithMesh(ModelRegistry.InternalMeshes[meshFilter.MeshIndex])
                    .WithColor(Color4.White)
                    .WithTransform(ref transform)
                    .Upload(RenderLayerDefinitions.PositionColorRenderLayer);
            }
        }
    }
}