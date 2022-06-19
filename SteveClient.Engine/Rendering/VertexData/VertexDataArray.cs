using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public class VertexDataArray
{
    private readonly float[] _buffer;
    private int _offset;
    
    public VertexDataArray(int bufferSize)
    {
        _buffer = new float[bufferSize];
    }

    private void Add(float value)
    {
        _buffer[_offset] = value;
        _offset++;
    }

    public VertexDataArray WithFloat(float item)
    {
        Add(item);

        return this;
    }

    public VertexDataArray WithVector2(Vector2 item)
    {
        Add(item.X);
        Add(item.Y);

        return this;
    }

    public VertexDataArray WithVector3(Vector3 item)
    {
        Add(item.X);
        Add(item.Y);
        Add(item.Z);

        return this;
    }

    public VertexDataArray WithVector4(Vector4 item)
    {
        Add(item.X);
        Add(item.Y);
        Add(item.Z);
        Add(item.Z);

        return this;
    }

    public static implicit operator float[](VertexDataArray right) => right._buffer;
}