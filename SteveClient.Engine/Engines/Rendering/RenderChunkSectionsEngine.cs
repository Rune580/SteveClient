using OpenTK.Mathematics;
using Serilog;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.Game;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils.ChunkSections;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Numerics;
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
                Vector3i sectionPos = new Vector3i(sectionComponent.ChunkPos.X, sectionComponent.SectionIndex, sectionComponent.ChunkPos.Y);

                if (sectionComponent.InRange)
                    continue;

                if (!_world.GetChunkSection(sectionPos).TrustEdges)
                    continue;

                if (!_world.IsChunkLoaded(sectionComponent.ChunkPos + Vector2i.UnitX) ||
                    !_world.IsChunkLoaded(sectionComponent.ChunkPos + Vector2i.UnitY) ||
                    !_world.IsChunkLoaded(sectionComponent.ChunkPos - Vector2i.UnitX) ||
                    !_world.IsChunkLoaded(sectionComponent.ChunkPos - Vector2i.UnitY))
                    continue;

                float distance = Vector3.Distance(sectionPos, _chunkSectionRenderer.PlayerPos);
                
                if (distance <= ClientSettings.RenderDistance + 1)
                {
                    if (!_world.LightMap.ContainsChunkSection(sectionPos))
                    {
                        UploadChunkLightData(sectionPos);
                        continue;
                    }
                }
                
                if (distance <= ClientSettings.RenderDistance)
                {
                    if (ChunkHasSurroundingLightMap(sectionPos))
                    {
                        Log.Verbose("Chunk Section {ChunkSectionPos} has surrounding lightmap data!", sectionPos);
                        
                        sectionComponent.ShouldRender = true;
                        sectionComponent.InRange = true;
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                ref var sectionComponent = ref sectionComponents[i];
                Vector3i sectionPos = new Vector3i(sectionComponent.ChunkPos.X, sectionComponent.SectionIndex, sectionComponent.ChunkPos.Y);
                
                float distance = Vector3.Distance(sectionPos, _chunkSectionRenderer.PlayerPos);
                if (distance > ClientSettings.RenderDistance)
                    sectionComponent.InRange = false;

                if (!sectionComponent.ShouldRender || !sectionComponent.InRange)
                    continue;
                
                sectionComponent.ShouldRender = false;

                _chunkSectionRenderer.EnqueueChunkSection(sectionPos);
            }
        }
    }

    private void UploadChunkLightData(Vector3i sectionPos)
    {
        ChunkSection chunkSection = _world.GetChunkSection(sectionPos);
        _world.LightMap.UploadLightData(sectionPos, chunkSection);
    }

    private bool ChunkHasSurroundingLightMap(Vector3i sectionPos)
    {
        return _world.LightMap.ContainsChunkSection(sectionPos.North()) &&
               _world.LightMap.ContainsChunkSection(sectionPos.East()) &&
               _world.LightMap.ContainsChunkSection(sectionPos.South()) &&
               _world.LightMap.ContainsChunkSection(sectionPos.West()) &&
               _world.LightMap.ContainsChunkSection(sectionPos.Above()) &&
               _world.LightMap.ContainsChunkSection(sectionPos.Below());
    }

    private void ReloadChunks()
    {
        if (!KeyBinds.ReloadChunks.IsPressed)
            return;
        
        _world.LightMap.Clear();
        
        RenderLayerDefinitions.OpaqueBlockLayer.ClearChunks();
        
        foreach (var ((sectionComponents, count), _) in entitiesDB.QueryEntities<ChunkSectionComponent>(GameGroups.ChunkSections.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var sectionComponent = ref sectionComponents[i];
                sectionComponent.ShouldRender = false;
                sectionComponent.InRange = false;
            }
        }
    }
}