using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Rendering.Shaders.Properties;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedModelQuad : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }
    public override IShaderProperty[] ShaderProperties { get; }

    private readonly string _textureResourceName;

    public BakedModelQuad(float[] vertices, uint[] indices, string textureResourceName, Matrix4 transform, IShaderProperty[] properties)
    {
        Vertices = vertices;
        Indices = indices;
        _textureResourceName = textureResourceName.Replace("minecraft:", "");
        Transform = transform;

        ShaderProperties = properties;
    }
    
    public BakedModelQuad(float[] vertices, uint[] indices, string textureResourceName, Matrix4 transform, Color4 tint)
        : this(vertices, indices, textureResourceName, transform, new IShaderProperty[] { new ColorShaderProperty("tint", tint) }) { }
    
    public BakedModelQuad(float[] vertices, uint[] indices, string textureResourceName, Matrix4 transform)
        : this(vertices, indices, textureResourceName, transform, Color4.White) { }

    public override bool HasTexture => true;
    public override void UseTexture()
    {
        TextureRegistry.GetTexture(_textureResourceName).Use();
    }
}