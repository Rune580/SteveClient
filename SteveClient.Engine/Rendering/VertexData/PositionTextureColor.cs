using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public class PositionTextureColor : IVertex
{
    public const int Stride = sizeof(float) * 9;
    public int GetStride() => Stride;
    
    public float[] VertexData { get; set; }

    public PositionTextureColor(Vector3 position, Vector2 texture, Vector4 color)
    {
        VertexData = new VertexDataArray(Stride)
            .WithVector3(position)
            .WithVector2(texture)
            .WithVector4(color);
    }
}