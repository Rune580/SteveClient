using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Models;

public readonly struct BakedModel
{
    public readonly float[] Vertices;
    public readonly uint[] Indices;
    public readonly Matrix4 Model;

    public BakedModel(float[] vertices, uint[] indices, Matrix4 model)
    {
        Vertices = vertices;
        Indices = indices;
        Model = model;
    }

    public int SizeOfVertices => Vertices.Length * sizeof(float);
    public int SizeOfIndices => Indices.Length * sizeof(uint);
}