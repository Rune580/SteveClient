using OpenTK.Mathematics;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine;

public class World
{
    public static World? ServerWorld;
    
    private readonly Dictionary<Vector2i, Chunk> _chunks = new();
    public readonly Dictionary<int, uint> MinecraftEntityIdMap = new();

    public World()
    {
        if (ServerWorld is not null)
            throw new Exception("There should only exist 1 world at any given moment!");
        
        ServerWorld = this;
    }

    public void LoadChunk(Chunk chunk)
    {
        _chunks[chunk.Position] = chunk;
    }

    public Chunk GetChunk(Vector2i pos)
    {
        return _chunks[pos];
    }

    public int GetBlockStateId(int x, int y, int z)
    {
        Vector2i chunkPos = ChunkPosFromBlockPos(x, z);

        Chunk chunk = GetChunk(chunkPos);

        Vector3i localPos = new Vector3i(x - chunkPos.X * 16, y, z - chunkPos.Y * 16);
        
        return chunk.GetBlockStateId(localPos);
    }

    public int GetBlockStateId(Vector3i worldPos)
    {
        return GetBlockStateId(worldPos.X, worldPos.Y, worldPos.Z);
    }

    private static Vector2i ChunkPosFromBlockPos(int x, int z)
    {
        int chunkX = (int)(x < 0 ? Math.Floor(x / 16f) : Math.Ceiling(x / 16f));
        int chunkZ = (int)(z < 0 ? Math.Floor(z / 16f) : Math.Ceiling(z / 16f));

        return new Vector2i(chunkX, chunkZ);
    }
}