using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Baked;

public class BakedRenderData : BaseBakedRenderData
{
    public override float[] Vertices { get; }
    public override uint[] Indices { get; }
    public override Matrix4 Transform { get; }

    public BakedRenderData(float[] vertices, uint[] indices, Matrix4 transform)
    {
        Vertices = vertices;
        Indices = indices;
        Transform = transform;
    }

    public override bool HasTexture => false;
    public override void UseTexture() => throw new NotSupportedException();
}