using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Game;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.VertexData;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Engine.Rendering.Utils.ChunkSections;

public readonly struct BakedChunkSection
{
    public readonly Vector3i ChunkPos;
    public readonly float[] Vertices;
    public readonly uint[] Indices;
    public readonly Matrix4 Transform;
    public readonly ChunkRenderLayer TargetLayer;

    private BakedChunkSection(Vector3i chunkPos, float[] vertices, uint[] indices, Matrix4 transform, ChunkRenderLayer targetLayer)
    {
        ChunkPos = chunkPos;
        Vertices = vertices;
        Indices = indices;
        Transform = transform;
        TargetLayer = targetLayer;
    }

    public static BakedChunkSection BakeChunkSection(World world, Vector3i sectionPos)
    {
        List<float> vertices = new List<float>();
        List<uint> indices = new List<uint>();

        GenerateQuads(world, sectionPos, vertices, indices);

        Vector3i worldPos = new Vector3i(sectionPos.X * 16, (sectionPos.Y - 4) * 16, sectionPos.Z * 16);

        Matrix4 transform = Matrix4.CreateTranslation(worldPos); 

        return new BakedChunkSection(sectionPos, vertices.ToArray(), indices.ToArray(), transform, RenderLayerDefinitions.OpaqueBlockLayer);
    }

    private static void GenerateQuads(World world, Vector3i sectionPos, in List<float> vertices, in List<uint> indices)
    {
        Vector3i worldPos = new Vector3i(sectionPos.X, (sectionPos.Y - 4), sectionPos.Z) * 16;
        for (int y = 0; y < 16; y++)
        {
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    Vector3i blockPos = new Vector3i(x, y, z) + worldPos;

                    GetQuadsForBlock(world, new Vector3i(x, y, z), blockPos, vertices, indices);
                }
            }
        }
    }

    private static void GetQuadsForBlock(World world, Vector3i localPos, Vector3i blockPos, in List<float> vertexList,
        in List<uint> indexList)
    {
        bool aboveOccluded = Occluded(world, blockPos, blockPos.Above());
        bool belowOccluded = Occluded(world, blockPos, blockPos.Below());
        bool northOccluded = Occluded(world, blockPos, blockPos.North());
        bool southOccluded = Occluded(world, blockPos, blockPos.South());
        bool eastOccluded = Occluded(world, blockPos, blockPos.East());
        bool westOccluded = Occluded(world, blockPos, blockPos.West());

        var blockStateId = world.GetBlockStateId(blockPos);

        if (blockStateId < 0)
            return;

        BlockState blockState = Blocks.GetBlockState(blockStateId);
        if (!blockState.TryGetBlockModel(out BlockModel blockModel))
            return;

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

            Vector3[] quadVertices =
            {
                blockModel.Vertices[modelQuad.Vertices[0]] + localPos,
                blockModel.Vertices[modelQuad.Vertices[1]] + localPos,
                blockModel.Vertices[modelQuad.Vertices[2]] + localPos,
                blockModel.Vertices[modelQuad.Vertices[3]] + localPos
            };

            uint offset = (uint)(vertexList.Count / PositionTextureAtlas.Size);

            float[] vertices = BakeVertexData(quadVertices, modelQuad.Uvs, modelQuad.TextureResourceName);
            uint[] indices = { offset + 0, offset + 2, offset + 1, offset + 3, offset + 1, offset + 2 };

            vertexList.AddRange(vertices);
            indexList.AddRange(indices);
        }
    }

    private static bool Occluded(World world, Vector3i currentPos, Vector3i neighborPos)
    {
        // Check to see we aren't doing this on an air block.
        int currentId = world.GetBlockStateId(currentPos);
        BlockState current = Blocks.GetBlockState(currentId);

        if (current.Air || current.Liquid)
            return false;

        int neighborId = world.GetBlockStateId(neighborPos);

        if (neighborId == -1)
            return true;

        BlockState neighbor = Blocks.GetBlockState(neighborId);

        if (!neighbor.Occludes)
            return false;

        // Check collision shapes
        
        // 1st do a simple check
        if (currentId == neighborId)
            return true;

        // Next do a full check
        return OcclusionShapeTest(current.OcclusionShape, neighbor.OcclusionShape, neighborPos - currentPos);
    }

    private static bool OcclusionShapeTest(VoxelShape current, VoxelShape neighbor, Vector3 dir)
    {
        Aabb curAabb = current.Closest(dir);
        Aabb neighborAabb = neighbor.Closest(-dir);

        Aabb curFace = curAabb.Face(dir);
        Aabb neighborFace = neighborAabb.Offset(dir).Face(-dir);

        return curFace == neighborFace;
    }

    private static float[] BakeVertexData(Vector3[] vertices, Vector2[] uvs, string textureResourceName)
    {
        //Vector3 tangent = CalculateTangent(vertices, uvs);
        int atlasLayer = TextureRegistry.BlockTextureAtlas.GetAtlasLayer(textureResourceName);
        
        IVertex[] vertexData = new IVertex[vertices.Length];

        for (int i = 0; i < vertexData.Length; i++)
            vertexData[i] = new PositionTextureAtlas(vertices[i], uvs[i], atlasLayer);

        return vertexData.VertexData();
    }

    private static Vector3 CalculateTangent(Vector3[] vertices, Vector2[] uvs)
    {
        Vector3 tangent = new Vector3();

        Vector3 edge1 = vertices[1] - vertices[0];
        Vector3 edge2 = vertices[2] - vertices[0];
        Vector2 deltaUv1 = uvs[1] - uvs[0];
        Vector2 deltaUv2 = uvs[2] - uvs[0];

        float f = 1f / (deltaUv1.X * deltaUv2.Y - deltaUv2.X * deltaUv1.Y);
        
        tangent.X = f * (deltaUv2.Y * edge1.X - deltaUv1.Y * edge2.X);
        tangent.Y = f * (deltaUv2.Y * edge1.Y - deltaUv1.Y * edge2.Y);
        tangent.Z = f * (deltaUv2.Y * edge1.Z - deltaUv1.Y * edge2.Z);

        return tangent;
    }
}