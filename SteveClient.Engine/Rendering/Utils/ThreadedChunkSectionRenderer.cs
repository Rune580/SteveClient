using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine.Rendering.Utils;

public class ThreadedChunkSectionRenderer
{
    private readonly World _world;
    private readonly ConcurrentQueue<ChunkSection> _chunkSections = new();
    private readonly Dictionary<Vector3i, BakedChunkSection> _bakedChunkSections = new();

    public ThreadedChunkSectionRenderer(World world)
    {
        _world = world;
    }

    private class BakedChunkSection
    {
        public readonly long Hash;
        public readonly BakedModelQuad[] BakedModelQuads;
        
        public BakedChunkSection(ChunkSection section)
        {
            Hash = section.GetContentHash();
            BakedModelQuads = GenerateQuads(section);
        }

        private static BakedModelQuad[] GenerateQuads(ChunkSection section)
        {
            List<BakedModelQuad> quads = new List<BakedModelQuad>();

            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        
                    }
                }
            }

            return quads.ToArray();
        }
    }
}