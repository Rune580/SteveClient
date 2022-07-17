using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.Shaders;

namespace SteveClient.Engine.Rendering.Definitions;

public static class ShaderDefinitions
{
    private static readonly ShaderAttribute PositionAttribute = new("aPosition", 3, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute NormalAttribute = new("aNormal", 3, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute ColorAttribute = new("aColor", 4, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute UvAttribute = new("aTexCoord", 2, VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute AtlasAttribute = new("aAtlas", 1,  VertexAttribPointerType.Float, false);
    private static readonly ShaderAttribute TangentAttribute = new("aTangent", 3, VertexAttribPointerType.Float, false);

    public static Shader PositionColorShader { get; private set; }
    public static Shader PositionTextureColorShader { get; private set; }
    public static Shader PositionTextureShader { get; private set; }
    public static Shader DefaultFontShader { get; private set; }
    public static Shader PosTexWireframeShader { get; private set; }
    public static Shader LineShader { get; private set; }
    public static Shader UiColorShader { get; private set; }
    
    public static Shader SolidBlockShader { get; private set; }
    
    public static Shader TextureInterpolatorShader { get; private set; }

    public static void LoadShaders()
    {
        PositionColorShader = new Shader("PositionColor.vert", "PositionColor.frag", PositionAttribute, ColorAttribute);
        PositionTextureColorShader = new Shader("PositionTextureColor", PositionAttribute, UvAttribute, ColorAttribute);
        PositionTextureShader = new Shader("PositionTexture", PositionAttribute, UvAttribute);
        DefaultFontShader = new Shader("DefaultFont", PositionAttribute, UvAttribute);
        
        PosTexWireframeShader = new Shader("Wireframe", PositionAttribute, UvAttribute);
        LineShader = new Shader("Line", PositionAttribute);

        UiColorShader = new Shader("DefaultUi", PositionAttribute);

        SolidBlockShader = new Shader("Game/SolidBlock", PositionAttribute, NormalAttribute, TangentAttribute, UvAttribute, AtlasAttribute);

        TextureInterpolatorShader = new Shader("Compute/TextureInterpolator");
    }
}