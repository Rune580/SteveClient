using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Numerics;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.Utils.ChunkSections;

public class ThreadedChunkSectionRenderer
{
    private const int MaxJobs = 8;
    
    private readonly World _world;
    private readonly ConcurrentQueue<Vector3i> _chunkSections = new();
    private readonly ConcurrentDictionary<Vector3i, BakedChunkSection> _bakedChunkSections = new();
    private readonly CancellationTokenSource _cts;
    
    private int _queueJobs;
    private Vector3i _playerPos;

    public Vector3i PlayerPos => _playerPos;
    
    public ThreadedChunkSectionRenderer(World world)
    {
        _world = world;

        _cts = new CancellationTokenSource();
        ThreadPool.QueueUserWorkItem(ProcessChunksMain, _cts);
    }

    ~ThreadedChunkSectionRenderer()
    {
        _cts.Cancel();
        _cts.Dispose();
    }

    public void EnqueueChunkSection(Vector3i sectionPos)
    {
        _chunkSections.Enqueue(sectionPos);
    }

    public void UpdatePlayerPos(Vector3 playerPos)
    {
        Vector3i blockPos = playerPos.AsBlockPos();
        
        Vector3i chunkPos = new Vector3i(blockPos.X / 16, (int)MathF.Floor((blockPos.Y + 64) / 16f), blockPos.Z / 16);

        if (chunkPos.X < 0)
            chunkPos.X--;

        if (chunkPos.Z < 0)
            chunkPos.Z--;

        _playerPos = chunkPos;
    }

    private void ProcessChunksMain(object? state)
    {
        if (state is not CancellationTokenSource token)
            throw new Exception();

        while (!token.IsCancellationRequested)
        {
            while (_queueJobs < MathF.Min(MaxJobs, _chunkSections.Count))
                ThreadPool.QueueUserWorkItem(ProcessQueue);

            Task.Delay(100).Wait();
        }
    }

    private void ProcessQueue(object? state)
    {
        Interlocked.Increment(ref _queueJobs);

        if (!_chunkSections.TryDequeue(out Vector3i sectionPos))
        {
            Interlocked.Decrement(ref _queueJobs);
            return;
        }

        // Check if we already have a chunk section baked out
        bool existingBakedSection = _bakedChunkSections.TryGetValue(sectionPos, out BakedChunkSection bakedSection);
        if (existingBakedSection)
        {
            // If we already have one, check it's hash against the chunk that's being requested.
            ChunkSection section = _world.GetChunkSection(sectionPos);
            
            if (section.GetContentHash() == bakedSection.Hash) // If the hashes match, then there is no need to re-bake the chunk.
            {
                Interlocked.Decrement(ref _queueJobs);
                return;
            }
            
            // Otherwise continue
        }
        
        // Bake out chunk section
        bakedSection = BakedChunkSection.BakeChunkSection(_world, sectionPos);

        // Add the newly baked section to the cache once we are not uploading.
        ThreadPool.QueueUserWorkItem(AddBakedChunkSection, new AddBakedChunkSectionState(sectionPos, bakedSection, existingBakedSection));

        Interlocked.Decrement(ref _queueJobs);
    }

    private void AddBakedChunkSection(object? state)
    {
        if (state is null)
            throw new NullReferenceException();

        if (state is not AddBakedChunkSectionState args)
            throw new InvalidCastException();
            
        AddBakedChunkSection(args.SectionPos, args.BakedSection, args.RemoveExisting);
    }

    private void AddBakedChunkSection(Vector3i sectionPos, BakedChunkSection bakedSection, bool removeExisting)
    {
        while (RenderLayerDefinitions.Rendering)
            Thread.Sleep(1);

        if (removeExisting)
            _bakedChunkSections.TryRemove(sectionPos, out _);

        _bakedChunkSections.TryAdd(sectionPos, bakedSection);

        if (bakedSection.Vertices.Length == 0)
            return;
        
        bakedSection.TargetLayer.UploadChunk(bakedSection);
    }

    private class AddBakedChunkSectionState
    {
        public readonly Vector3i SectionPos;
        public readonly BakedChunkSection BakedSection;
        public readonly bool RemoveExisting;
        
        public AddBakedChunkSectionState(Vector3i sectionPos, BakedChunkSection bakedSection, bool removeExisting)
        {
            SectionPos = sectionPos;
            BakedSection = bakedSection;
            RemoveExisting = removeExisting;
        }
    }
}