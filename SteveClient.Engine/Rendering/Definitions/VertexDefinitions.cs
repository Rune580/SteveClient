using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.ShaderDefinitions;

namespace SteveClient.Engine.Rendering.Definitions;

public static class VertexDefinitions
{
    public static VertexDefinition<PositionColor> PositionColorTriangles;
    public static VertexDefinition<PositionTextureColor> PositionTextureColorTriangles;
    public static VertexDefinition<PositionTexture> PositionTextureTriangles;
    public static VertexDefinition<PositionTexture> DefaultFont;
    public static VertexDefinition<Position> Lines;

    public static void Init()
    {
        PositionColorTriangles = new VertexDefinition<PositionColor>(PositionColorShader, PrimitiveType.Triangles);
        PositionTextureColorTriangles = new VertexDefinition<PositionTextureColor>(PositionTextureColorShader, PrimitiveType.Triangles);
        PositionTextureTriangles = new VertexDefinition<PositionTexture>(PositionTextureShader, PrimitiveType.Triangles);
        DefaultFont = new VertexDefinition<PositionTexture>(DefaultFontShader, PrimitiveType.Triangles);
        Lines = new VertexDefinition<Position>(PositionColorShader, PrimitiveType.Lines);
    }
    
    public readonly struct VertexDefinition<TVertex> where TVertex : IVertex
    {
        public readonly Shader Shader;
        public readonly PrimitiveType PrimitiveType;
        public readonly int VertexSize;

        public VertexDefinition(Shader shader, PrimitiveType primitiveType) : this()
        {
            Shader = shader;
            PrimitiveType = primitiveType;

            TVertex vertex = (TVertex)Activator.CreateInstance(VertexType)!;
            VertexSize = vertex.GetSize();
        }

        public Type VertexType => typeof(TVertex);
    }
}