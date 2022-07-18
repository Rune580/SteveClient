using OpenTK.Mathematics;
using SteveClient.Assimp;
using SteveClient.Engine.AssetManagement;

namespace SteveClient.Engine.Rendering.Models;

public readonly struct InternalMesh
{
    public readonly Vector3[] Vertices;
    public readonly Vector3[] Normals;
    public readonly uint[] Indices;
    public readonly int Index;

    public InternalMesh(Mesh mesh)
    {
        Vertices = mesh.Vertices;
        Normals = mesh.Normals;
        Indices = mesh.Indices;
        
        Index = ModelRegistry.InternalMeshes.Count;
        ModelRegistry.InternalMeshes.Add(this);
    }
}