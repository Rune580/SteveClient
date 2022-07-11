using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.Shaders.Properties;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedLine : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }
    public override IShaderProperty[] ShaderProperties { get; }

    public BakedLine(float[] vertices, Matrix4 transform, Color4 color)
    {
        Vertices = vertices;
        Indices = new uint[]{ 0, 1 };
        Transform = transform;

        ShaderProperties = new IShaderProperty[]
        {
            new ColorShaderProperty("color", color)
        };
    }
    
    public BakedLine(float[] vertices, Color4 color) : this(vertices, Matrix4.CreateTranslation(0, 0, 0), color) { }
}