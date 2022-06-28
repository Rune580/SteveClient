using OpenTK.Mathematics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct ChunkSectionComponent : IEntityComponent
{
    public Vector2i ChunkPos;
    public int SectionIndex;

    public ChunkSectionComponent(Vector2i chunkPos, int sectionIndex)
    {
        ChunkPos = chunkPos;
        SectionIndex = sectionIndex;
    }
}