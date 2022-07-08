using OpenTK.Graphics.OpenGL4;

namespace SteveClient.Engine.Rendering.Shaders;

public readonly struct ShaderAttribute
{
    public readonly string Name;
    public readonly int Size;
    public readonly VertexAttribPointerType VertexAttribPointerType;
    public readonly bool Normalized;

    public ShaderAttribute(string name, int size, VertexAttribPointerType vertexAttribPointerType, bool normalized)
    {
        Name = name;
        Size = size;
        VertexAttribPointerType = vertexAttribPointerType;
        Normalized = normalized;
    }
}