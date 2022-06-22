using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Models;

public readonly struct SimpleModel
{
    public readonly Vector3[] Vertices;
    public readonly uint[] Indices;
    public readonly int Index;

    public SimpleModel(Vector3[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;
        Index = ModelRegistry.Models.Count;
        
        ModelRegistry.Models.Add(this);
    }
}