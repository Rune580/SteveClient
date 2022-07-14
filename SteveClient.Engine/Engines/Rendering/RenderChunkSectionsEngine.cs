using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils.ChunkSections;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Rendering;

public class RenderChunkSectionsEngine : RenderingEngine
{
    private readonly World _world;
    private readonly ThreadedChunkSectionRenderer _chunkSectionRenderer;

    public RenderChunkSectionsEngine(World world)
    {
        _world = world;
        _chunkSectionRenderer = new ThreadedChunkSectionRenderer(_world);
    }

    public override void Execute(float delta)
    {
        ReloadChunks();
        
        var optional = entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.Player.BuildGroup);
        if (!optional.HasValue)
            return;

        ref var playerTransform = ref optional.Get1();
        
        _chunkSectionRenderer.UpdatePlayerPos(playerTransform.Position);
        
        foreach (var ((sectionComponents, count), _) in entitiesDB.QueryEntities<ChunkSectionComponent>(GameGroups.ChunkSections.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var sectionComponent = ref sectionComponents[i];

                if (!sectionComponent.ShouldRender)
                    continue;

                sectionComponent.ShouldRender = false;
                
                Vector3i sectionPos = new Vector3i(sectionComponent.ChunkPos.X, sectionComponent.SectionIndex, sectionComponent.ChunkPos.Y);
                float distance = Vector3.Distance(sectionPos, _chunkSectionRenderer.PlayerPos);

                if (distance > 6)
                    continue;
                
                _chunkSectionRenderer.EnqueueChunkSection(sectionPos);
            }
        }
    }

    private void ReloadChunks()
    {
        if (!KeyBinds.ReloadChunks.IsPressed)
            return;
        
        RenderLayerDefinitions.OpaqueBlockLayer.ClearChunks();
        
        foreach (var ((sectionComponents, count), _) in entitiesDB.QueryEntities<ChunkSectionComponent>(GameGroups.ChunkSections.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var sectionComponent = ref sectionComponents[i];
                sectionComponent.ShouldRender = true;
            }
        }
    }
}