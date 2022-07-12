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

        List<BlockModel.Quad> quads = new List<BlockModel.Quad>();

        foreach (var quad in _quads)
        {
            int i0 = vertices.IndexOf(quad.Vertices[0]);
            int i1 = vertices.IndexOf(quad.Vertices[1]);
            int i2 = vertices.IndexOf(quad.Vertices[2]);
            int i3 = vertices.IndexOf(quad.Vertices[3]);

            if (i0 < 0)
            {
                i0 = vertices.Count;
                vertices.Add(quad.Vertices[0]);
            }

            if (i1 < 0)
            {
                i1 = vertices.Count;
                vertices.Add(quad.Vertices[1]);
            }

            if (i2 < 0)
            {
                i2 = vertices.Count;
                vertices.Add(quad.Vertices[2]);
            }

            if (i3 < 0)
            {
                i3 = vertices.Count;
                vertices.Add(quad.Vertices[3]);
            }
            
            triangles.Add((uint)i0);
            triangles.Add((uint)i2);
            triangles.Add((uint)i1);
            triangles.Add((uint)i3);
            triangles.Add((uint)i1);
            triangles.Add((uint)i2);

            int[] quadVerts = { i0, i1, i2, i3 };

            BlockModel.Quad bakedQuad = new BlockModel.Quad(quadVerts, quad.Uvs, quad.TextureResourceName, quad.CullFace);
            quads.Add(bakedQuad);
        }

        BlockModel result = new BlockModel(vertices.ToArray(), triangles.ToArray(), quads.ToArray());
        
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
            new Vector2(blockFace.UvMin.X, blockFace.UvMax.Y),
            blockFace.UvMax,
            blockFace.UvMin,
            new Vector2(blockFace.UvMax.X, blockFace.UvMin.Y)
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
}