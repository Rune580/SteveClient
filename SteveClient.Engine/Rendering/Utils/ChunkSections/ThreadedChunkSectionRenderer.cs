using System.Collections.Concurrent;
using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.VertexData;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.Utils.ChunkSections;

public class ThreadedChunkSectionRenderer
{
    private readonly World _world;
    private readonly ConcurrentQueue<Vector3i> _chunkSections = new();
    private readonly ConcurrentDictionary<Vector3i, BakedChunkSection> _bakedChunkSections = new();
    private readonly CancellationTokenSource _cts;
    
    private bool _uploading;
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

    public void Upload()
    {
        _uploading = true;
        
        foreach (var (_, bakedSection) in _bakedChunkSections)
        {
            foreach (var quad in bakedSection.BakedModelQuads)
                RenderLayerDefinitions.SolidBlockLayer.UploadRenderData(quad);
        }

        _uploading = false;
    }

    private void ProcessChunksMain(object? state)
    {
        if (state is not CancellationTokenSource token)
            throw new Exception();

        while (!token.IsCancellationRequested)
        {
            while (_queueJobs < MathF.Min(4, _chunkSections.Count))
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
        while (_uploading)
            Thread.Sleep(1);

        if (removeExisting)
            _bakedChunkSections.TryRemove(sectionPos, out _);

        _bakedChunkSections.TryAdd(sectionPos, bakedSection);
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

    private readonly struct BakedChunkSection
    {
        public readonly long Hash;
        public readonly BakedModelQuad[] BakedModelQuads;
        
        private BakedChunkSection(long hash, BakedModelQuad[] bakedModelQuads)
        {
            Hash = hash;
            BakedModelQuads = bakedModelQuads;
        }

        public static BakedChunkSection BakeChunkSection(World world, Vector3i sectionPos)
        {
            ChunkSection section = world.GetChunkSection(sectionPos);

            return new BakedChunkSection(section.GetContentHash(), GenerateQuads(world, sectionPos));
        }

        private static BakedModelQuad[] GenerateQuads(World world, Vector3i sectionPos)
        {
            List<BakedModelQuad> quads = new List<BakedModelQuad>();

            for (int y = 0; y < 16; y++)
            {
                for (int z = 0; z < 16; z++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        Vector3i blockPos = 
                            new Vector3i(x + sectionPos.X * 16, (y + sectionPos.Y * 16) - 64, z + sectionPos.Z * 16);
                        
                        GetQuadsForBlock(world, blockPos, quads);
                    }
                }
            }

            return quads.ToArray();
        }

        private static void GetQuadsForBlock(World world, Vector3i blockPos, in List<BakedModelQuad> bakedQuads)
        {
            bool aboveOccluded = Occluded(world, blockPos.Above());
            bool belowOccluded = Occluded(world, blockPos.Below());
            bool northOccluded = Occluded(world, blockPos.North());
            bool southOccluded = Occluded(world, blockPos.South());
            bool eastOccluded = Occluded(world, blockPos.East());
            bool westOccluded = Occluded(world, blockPos.West());

            BlockState blockState = world.GetBlockState(blockPos);
            if (!blockState.TryGetBlockModel(out BlockModel blockModel))
                return;

            VertexFactory factory = GetVertexFactory();

            foreach (var modelQuad in blockModel.Quads)
            {
                // Exclude if occluded and the face has matching cull direction
                if (aboveOccluded && modelQuad.CullFace == Directions.Up)
                    continue;
                if (belowOccluded && modelQuad.CullFace == Directions.Down)
                    continue;
                if (northOccluded && modelQuad.CullFace == Directions.North)
                    continue;
                if (southOccluded && modelQuad.CullFace == Directions.South)
                    continue;
                if (eastOccluded && modelQuad.CullFace == Directions.East)
                    continue;
                if (westOccluded && modelQuad.CullFace == Directions.West)
                    continue;
                
                Vector3[] vertices =
                {
                    blockModel.Vertices[modelQuad.Vertices[0]],
                    blockModel.Vertices[modelQuad.Vertices[1]],
                    blockModel.Vertices[modelQuad.Vertices[2]],
                    blockModel.Vertices[modelQuad.Vertices[3]]
                };

                float[] vertexData = factory.Consume(vertices, null, null, modelQuad.Uvs).VertexData();

                BakedModelQuad quad = new BakedModelQuad(vertexData, new uint[] { 0, 2, 1, 3, 1, 2 },
                    modelQuad.TextureResourceName, Matrix4.CreateTranslation(new Vector3(blockPos.X + 16, blockPos.Y - 32, blockPos.Z + 32)));
                
                bakedQuads.Add(quad);
            }
        }

        private static bool Occluded(World world, Vector3i blockPos)
        {
            int id = world.GetBlockStateId(blockPos);
            
            if (id == -1)
                return true;

            return Blocks.GetBlockState(id).Occludes;
        }

        private static VertexFactory GetVertexFactory()
        {
            return RenderLayerDefinitions.SolidBlockLayer.GetVertexFactory();
        } 
    }
}