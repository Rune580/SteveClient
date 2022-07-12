using OpenTK.Mathematics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct ChunkSectionComponent : IEntityComponent
{
    public Vector2i ChunkPos;
    public readonly int SectionIndex;
    public bool ShouldRender;

    public ChunkSectionComponent(Vector2i chunkPos, int sectionIndex, bool shouldRender = false)
    {
        ChunkPos = chunkPos;
        SectionIndex = sectionIndex;
        ShouldRender = shouldRender;
    }
}