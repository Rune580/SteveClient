using OpenTK.Graphics.OpenGL4;
using static SteveClient.Engine.Rendering.Shader;

namespace SteveClient.Engine.Rendering.Definitions;

public static class ShaderDefinitions
{
    private static readonly ShaderAttribute PositionAttribute = new("aPosition", 3, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute ColorAttribute = new("aColor", 4, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute UvAttribute = new("aTexCoord", 2, VertexAttribPointerType.Float, false);

    public static Shader PositionColorShader { get; private set; }
    public static Shader PositionTextureColorShader { get; private set; }

    public static void LoadShaders()
    {
        PositionColorShader = new Shader("PositionColor.vert", "PositionColor.frag", PositionAttribute, ColorAttribute);
        PositionTextureColorShader = new Shader("PositionTextureColor", PositionAttribute, UvAttribute, ColorAttribute);
    }
}