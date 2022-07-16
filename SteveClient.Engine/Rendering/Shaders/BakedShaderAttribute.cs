using OpenTK.Graphics.OpenGL4;

namespace SteveClient.Engine.Rendering.Shaders;

public readonly struct BakedShaderAttribute
{
    public readonly int Location;
    public readonly int Size;
    public readonly VertexAttribPointerType VertexAttribPointerType;
    public readonly bool Normalized;
    public readonly int Stride;
    public readonly int Offset;

    public readonly bool IsIPointer;
    public VertexAttribIntegerType VertexAttribIntegerType => (VertexAttribIntegerType)VertexAttribPointerType;

    public BakedShaderAttribute(int location, int size, VertexAttribPointerType vertexAttribPointerType, bool normalized, int stride, int offset)
    {
        Location = location;
        Size = size;
        VertexAttribPointerType = vertexAttribPointerType;
        Normalized = normalized;
        Stride = stride;
        Offset = offset;

        IsIPointer = vertexAttribPointerType is VertexAttribPointerType.Int
            or VertexAttribPointerType.UnsignedInt;
    }
}