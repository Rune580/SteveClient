using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.Shaders.Properties;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedQuad : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }
    public override IShaderProperty[] ShaderProperties { get; }
    
    public BakedQuad(float[] vertices, uint[] indices, Matrix4 transform, Color4 color)
    {
        Vertices = vertices;
        Indices = indices;
        Transform = transform;

        ShaderProperties = new IShaderProperty[]
        {
            new ColorShaderProperty("color", color)
        };
    }
}