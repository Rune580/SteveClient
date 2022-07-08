using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders;

namespace SteveClient.Engine.Rendering.Baked;

public interface IBakedRenderData
{
    public float[] Vertices { get; }
    public uint[] Indices { get; }
    public Matrix4 Transform { get; }

    public bool HasTexture { get; }
    public void UseTexture();
    
    public bool HasShaderProperties { get; }
    public void ApplyShaderProperties(Shader shader);
    
    public int SizeOfVertices { get; }
    public int SizeOfIndices { get; }
}