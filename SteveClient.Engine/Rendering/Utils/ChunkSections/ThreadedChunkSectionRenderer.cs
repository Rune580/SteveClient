using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Engine.Game;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Numerics;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.Utils.ChunkSections;

public class ThreadedChunkSectionRenderer
{
    private const int MaxJobs = 4;
    
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
        Task.Run(ProcessChunksMain);
        //ThreadPool.QueueUserWorkItem(ProcessChunksMain, _cts);
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
        
        Vector3i chunkPos = new Vector3i((int)MathF.Floor(blockPos.X / 16f), (int)MathF.Floor(blockPos.Y / 16f) + 4, (int)MathF.Floor(blockPos.Z / 16f));

        _playerPos = chunkPos;
    }

    private async void ProcessChunksMain()
    {
        var token = _cts.Token;
        
        while (!token.IsCancellationRequested)
        {
            while (_queueJobs < MathF.Min(MaxJobs, _chunkSections.Count))
                ThreadPool.QueueUserWorkItem(ProcessQueue);

            await Task.Delay(20, token);
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