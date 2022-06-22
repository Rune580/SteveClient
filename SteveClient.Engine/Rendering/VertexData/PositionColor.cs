using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct PositionColor : IVertex
{
    public const int Size = 3 + 4;
    public const int Stride = sizeof(float) * Size;
    public int GetStride() => Stride;
    public int GetSize() => Size;
    public float[] VertexData { get; set; }

    public PositionColor(Vector3 position, Color4 color)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position)
            .WithColor4(color);
    }
}