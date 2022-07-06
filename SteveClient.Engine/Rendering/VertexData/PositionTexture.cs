using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct PositionTexture : IVertex
{
    public const int Size = 3 + 2;
    public const int Stride = sizeof(float) * Size;
    public int GetStride() => Stride;
    public int GetSize() => Size;
    public float[] VertexData { get; set; }

    public PositionTexture(Vector3 position, Vector2 texture)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position)
            .WithVector2(texture);
    }
}