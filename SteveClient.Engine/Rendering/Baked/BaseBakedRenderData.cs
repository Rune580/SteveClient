using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Baked;

public abstract class BaseBakedRenderData : IBakedRenderData
{
    public abstract float[] Vertices { get; }
    public abstract uint[] Indices { get; }
    public abstract Matrix4 Transform { get; }
    
    public abstract bool HasTexture { get; }
    public abstract void UseTexture();

    public int SizeOfVertices => Vertices.Length * sizeof(float);
    public int SizeOfIndices => Indices.Length * sizeof(uint);
}