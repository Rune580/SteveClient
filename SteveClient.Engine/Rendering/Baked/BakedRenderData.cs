using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders.Properties;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedRenderData : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }
    public override IShaderProperty[] ShaderProperties { get; }

    public BakedRenderData(float[] vertices, uint[] indices, Matrix4 transform, IShaderProperty[] properties)
    {
        Vertices = vertices;
        Indices = indices;
        Transform = transform;
        ShaderProperties = properties;
    }
    
    public BakedRenderData(float[] vertices, uint[] indices, Matrix4 transform)
        : this(vertices, indices, transform, Array.Empty<IShaderProperty>()) { }

    public override bool HasTexture => false;
    public override void UseTexture() => throw new NotSupportedException();
}