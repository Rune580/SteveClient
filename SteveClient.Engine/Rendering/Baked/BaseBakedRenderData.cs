using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Baked;

public abstract class BaseBakedRenderData : IBakedRenderData
{
    public abstract float[] Vertices { get; }
    public abstract uint[] Indices { get; }
    public abstract Matrix4 Transform { get; }

    public virtual bool HasTexture { get; } = false;
    public virtual void UseTexture()
    {
        throw new NotSupportedException();
    }

    public virtual bool HasShaderProperties { get; } = false;
    public virtual void ApplyShaderProperties(Shader shader)
    {
        throw new NotSupportedException();
    }

    public int SizeOfVertices => Vertices.Length * sizeof(float);
    public int SizeOfIndices => Indices.Length * sizeof(uint);
}