using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedQuad : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }

    private readonly string _textureResourceName;

    public BakedQuad(float[] vertices, uint[] indices, string textureResourceName, Matrix4 transform)
    {
        Vertices = vertices;
        Indices = indices;
        _textureResourceName = textureResourceName.Replace("minecraft:", "");
        Transform = transform;
    }

    public override bool HasTexture => true;
    public override void UseTexture()
    {
        TextureRegistry.GetTexture(_textureResourceName).Use();
    }
}