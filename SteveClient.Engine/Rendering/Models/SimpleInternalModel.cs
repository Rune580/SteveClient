using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;

namespace SteveClient.Engine.Rendering.Models;

public readonly struct SimpleInternalModel
{
    public readonly Vector3[] Vertices;
    public readonly uint[] Indices;
    public readonly int Index;

    public SimpleInternalModel(Vector3[] vertices, uint[] indices)
    {
        Vertices = vertices;
        Indices = indices;
        Index = ModelRegistry.SimpleInternalModels.Count;
        
        ModelRegistry.SimpleInternalModels.Add(this);
    }
}