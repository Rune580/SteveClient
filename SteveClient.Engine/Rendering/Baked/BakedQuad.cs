using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Shaders;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedQuad : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }

    private Color4 _color;
    
    public BakedQuad(float[] vertices, uint[] indices, Matrix4 transform, Color4 color)
    {
        Vertices = vertices;
        Indices = indices;
        Transform = transform;

        _color = color;
    }

    public override bool HasShaderProperties => true;

    public override void ApplyShaderProperties(Shader shader)
    {
        shader.SetColor("color", _color);
    }
}