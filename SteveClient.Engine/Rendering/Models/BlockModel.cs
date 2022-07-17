using OpenTK.Mathematics;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Engine.Rendering.Models;

public readonly struct BlockModel
{
    public readonly Vector3[] Vertices;
    public readonly uint[] Indices;
    public readonly Vector3[] Normals;
    public readonly Quad[] Quads;

    public BlockModel(Vector3[] vertices, uint[] indices, Vector3[] normals, Quad[] quads)
    {
        Vertices = vertices;
        Indices = indices;
        Normals = normals;
        Quads = quads;
    }

    public Vector3[] QuadVertices
    {
        get
        {
            Vector3[] result = new Vector3[Quads.Length * 4];

            int i = 0;
            foreach (var quad in Quads)
            {
                result[i] = Vertices[quad.Vertices[0]];
                result[i + 1] = Vertices[quad.Vertices[1]];
                result[i + 2] = Vertices[quad.Vertices[2]];
                result[i + 3] = Vertices[quad.Vertices[3]];

                i += 4;
            }

            return result;
        }
    }
    
    public readonly struct Quad
    {
        public readonly int[] Vertices;
        public readonly Vector2[] Uvs;
        public readonly int Normal;
        public readonly string TextureResourceName;
        public readonly Directions CullFace;

        public Quad(int[] vertices, Vector2[] uvs, int normal, string textureResourceName, Directions cullFace = Directions.None)
        {
            Vertices = vertices;
            Normal = normal;
            Uvs = uvs;
            TextureResourceName = textureResourceName;
            CullFace = cullFace;
        }
    }
}