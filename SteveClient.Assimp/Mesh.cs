using OpenTK.Mathematics;

namespace SteveClient.Assimp;

public class Mesh
{
    public Vector3[] Vertices { get; }
    public Vector3[] Normals { get; }
    public uint[] Indices { get; }
    
    internal Mesh(Vector3[] vertices, Vector3[] normals, uint[] indices)
    {
        Vertices = vertices;
        Normals = normals;
        Indices = indices;
    }
}