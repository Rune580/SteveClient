using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.UnBaked;
using SteveClient.Minecraft.Data.Schema.BlockStates;
using SteveClient.Minecraft.ModelLoading;
using SteveClient.Minecraft.Numerics;

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

    public BlockModelBuilder AddQuad(RawBlockFace blockFace, VariantModelJson? modelJson = null)
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

        if (modelJson is not null)
            ApplyVariant(quad, modelJson);
        
        _quads.Add(quad);
        
        return this;
    }

    public BlockModelBuilder AddRawBlockModel(RawBlockModel blockModel, VariantModelJson? modelJson = null)
    {
        foreach (var blockFace in blockModel.Faces)
            AddQuad(blockFace, modelJson);
        
        return this;
    }

    private static void ApplyVariant(in TexturedQuad quad, in VariantModelJson json)
    {
        bool uvLock = json.UvLock ?? false;
        
        if (json.X.HasValue)
        {
            float xRad = json.X.Value * (MathF.PI / 180f);
            Matrix3 xRot = Matrix3.CreateRotationX(xRad);

            ApplyRotation(quad.Vertices, xRot);

            if (uvLock)
            {
                Matrix2 rot = Matrix2.CreateRotation(-xRad);
                //ApplyRotation(quad.Uvs, rot);
            }
        }

        if (json.Y.HasValue)
        {
            quad.CullFace = quad.CullFace.RotateAroundY(json.Y.Value);
            
            float yRad = json.Y.Value * (MathF.PI / 180f);
            Matrix3 yRot = Matrix3.CreateRotationY(yRad);
            
            ApplyRotation(quad.Vertices, yRot);

            if (uvLock && SameAxis(quad.Vertices, Vector3.UnitY))
            {
                Matrix2 rot = Matrix2.CreateRotation(-yRad);
                ApplyRotation(quad.Uvs, rot);
            }
        }
    }

    private static void ApplyTranslation(in Vector3[] vertices, Vector3 translation)
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] += translation;
    }

    private static void ApplyRotation(in Vector3[] vertices, Matrix3 rotation)
    {
        Vector3 origin = new Vector3(0.5f, 0.5f, 0.5f);
        
        ApplyTranslation(vertices, -origin);
        
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * vertices[i];
        
        ApplyTranslation(vertices, origin);
    }

    private static void ApplyTranslation(in Vector2[] vertices, Vector2 translation)
    {
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] += translation;
    }
    
    private static void ApplyRotation(in Vector2[] vertices, Matrix2 rotation)
    {
        Vector2 origin = new Vector2(0.5f, 0.5f);
        
        ApplyTranslation(vertices, -origin);
        
        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = rotation * vertices[i];
        
        ApplyTranslation(vertices, origin);
    }

    private static bool SameAxis(in Vector3[] vertices, Vector3 axis)
    {
        Vector3[] temp = new Vector3[vertices.Length];

        for (int i = 0; i < temp.Length; i++)
        {
            Vector3 vertex = TruncateVertex(vertices[i]);
            temp[i] = (vertex + Vector3.One) * axis;
        }

        var val = temp.First();

        return temp.All(vertex => vertex == val);
    }

    private static Vector3 TruncateVertex(in Vector3 vertex, int digits = 4)
    {
        return new Vector3((float)Math.Round(vertex.X, digits, MidpointRounding.ToEven),
            (float)Math.Round(vertex.Y, digits, MidpointRounding.ToEven),
            (float)Math.Round(vertex.Z, digits, MidpointRounding.ToEven));
    }

    private static Vector3 CalculateNormal(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Cross(b - a, c - a).Normalized();
    }
}