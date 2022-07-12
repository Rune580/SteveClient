using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.Shaders.Properties;

namespace SteveClient.Engine.Rendering.Baked;

public abstract class BaseBakedRenderData : IBakedRenderData
{
    public abstract float[] Vertices { get; }
    public abstract uint[] Indices { get; }
    public abstract Matrix4 Transform { get; }
    public abstract IShaderProperty[] ShaderProperties { get; }

    public virtual bool HasTexture { get; } = false;
    public virtual void UseTexture()
    {
        throw new NotSupportedException();
    }

    public int SizeOfVertices => Vertices.Length * sizeof(float);
    public int SizeOfIndices => Indices.Length * sizeof(uint);
    
    public bool HasShaderProperties => ShaderProperties.Length > 0;
    public void ApplyShaderProperties(Shader shader)
    {
        foreach (var shaderProperty in ShaderProperties)
            shaderProperty.Apply(shader);
    }

    public virtual IBakedRenderData Clone()
    {
        return this;
    }
}