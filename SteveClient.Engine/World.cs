using OpenTK.Mathematics;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine;

public class World
{
    private readonly Dictionary<Vector2i, Chunk> _chunks = new();
    public readonly Dictionary<int, uint> MinecraftEntityIdMap = new();

    public void LoadChunk(Chunk chunk)
    {
        _chunks[chunk.Position] = chunk;
    }

    public Chunk GetChunk(Vector2i pos)
    {
        return _chunks[pos];
    }
}