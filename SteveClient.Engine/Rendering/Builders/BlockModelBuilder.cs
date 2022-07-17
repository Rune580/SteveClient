using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.UnBaked;
using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Engine.Rendering.Builders;

public class BlockModelBuilder
{
    private readonly List<TexturedQuad> _quads;

    public BlockModelBuilder()
    {
        _quads = new List<TexturedQuad>();
    }

    public BlockModel Build()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<uint> triangles = new List<uint>();
        List<Vector3> normals = new List<Vector3>();

        List<BlockModel.Quad> quads = new List<BlockModel.Quad>();

        foreach (var quad in _quads)
        {
            int v0 = vertices.IndexOf(quad.Vertices[0]);
            int v1 = vertices.IndexOf(quad.Vertices[1]);
            int v2 = vertices.IndexOf(quad.Vertices[2]);
            int v3 = vertices.IndexOf(quad.Vertices[3]);

            if (v0 < 0)
            {
                v0 = vertices.Count;
                vertices.Add(quad.Vertices[0]);
            }

            if (v1 < 0)
            {
                v1 = vertices.Count;
                vertices.Add(quad.Vertices[1]);
            }

            if (v2 < 0)
            {
                v2 = vertices.Count;
                vertices.Add(quad.Vertices[2]);
            }

            if (v3 < 0)
            {
                v3 = vertices.Count;
                vertices.Add(quad.Vertices[3]);
            }
            
            triangles.Add((uint)v0);
            triangles.Add((uint)v2);
            triangles.Add((uint)v1);
            triangles.Add((uint)v3);
            triangles.Add((uint)v1);
            triangles.Add((uint)v2);

            Vector3 normal = CalculateNormal(quad.Vertices[0], quad.Vertices[1], quad.Vertices[2]);
            int n = normals.IndexOf(normal);
            
            if (n < 0)
            {
                n = normals.Count;
                normals.Add(normal);
            }

            int[] quadVerts = { v0, v1, v2, v3 };

            BlockModel.Quad bakedQuad = new BlockModel.Quad(quadVerts, quad.Uvs, n, quad.TextureResourceName, quad.CullFace);
            quads.Add(bakedQuad);
        }

        BlockModel result = new BlockModel(vertices.ToArray(), triangles.ToArray(), normals.ToArray(), quads.ToArray());
        
        _quads.Clear();

        return result;
    }

    public BlockModelBuilder AddQuad(RawBlockFace blockFace)
    {
        TexturedQuad quad = new TexturedQuad(new[]
        {
            blockFace.TopLeft,
            blockFace.TopRight,
            blockFace.BottomLeft,
            blockFace.BottomRight
        }, new []
        {
            blockFace.Xy,
            blockFace.Uy,
            blockFace.Xv,
            blockFace.Uv
        }, blockFace.Texture, blockFace.CullFace);
        
        _quads.Add(quad);
        
        return this;
    }

    public BlockModelBuilder AddRawBlockModel(RawBlockModel blockModel)
    {
        foreach (var blockFace in blockModel.Faces)
            AddQuad(blockFace);
        
        return this;
    }

    private static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Cross(b - a, c - a).Normalized();
    }
}