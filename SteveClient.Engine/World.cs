﻿using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Data;

namespace SteveClient.Engine;

public class World
{
    public static World? ServerWorld;
    
    private readonly ConcurrentDictionary<Vector2i, Chunk> _chunks = new();
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

    public ChunkSection GetChunkSection(Vector3i sectionPos)
    {
        Chunk chunk = GetChunk(new Vector2i(sectionPos.X, sectionPos.Z));
        return chunk.GetChunkSection(sectionPos.Y);
    }

    public int GetBlockStateId(int x, int y, int z)
    {
        Vector2i chunkPos = ChunkPosFromBlockPos(x, z);

        if (!_chunks.TryGetValue(chunkPos, out Chunk? chunk))
            return -1;

        Vector3i localPos = new Vector3i(x - chunkPos.X * 16, y, z - chunkPos.Y * 16);

        if (localPos.X < 0)
            localPos.X = 16 + localPos.X;
        if (localPos.Y < 0)
            localPos.Y = 16 + localPos.Y;
        if (localPos.Z < 0)
            localPos.Z = 16 + localPos.Z;
        
        return chunk.GetBlockStateId(localPos);
    }

    public int GetBlockStateId(Vector3i worldPos)
    {
        return GetBlockStateId(worldPos.X, worldPos.Y, worldPos.Z);
    }

    public ref readonly BlockState GetBlockState(int x, int y, int z)
    {
        int id = GetBlockStateId(x, y, z);

        return ref Blocks.GetBlockState(id);
    }

    public ref readonly BlockState GetBlockState(Vector3i worldPos)
    {
        return ref GetBlockState(worldPos.X, worldPos.Y, worldPos.Z);
    }

    private static Vector2i ChunkPosFromBlockPos(int x, int z)
    {
        int chunkX = (int)(x < 0 ? Math.Floor(x / 16f) : Math.Ceiling(x / 16f));
        int chunkZ = (int)(z < 0 ? Math.Floor(z / 16f) : Math.Ceiling(z / 16f));

        return new Vector2i(chunkX, chunkZ);
    }
}