using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct Position : IVertex
{
    public const int Size = 3;
    public const int Stride = sizeof(float) * Size;
    public int GetStride() => Stride;
    public int GetSize() => Size;
    public float[] VertexData { get; set; }

    public Position(Vector3 position)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position);
    }
}