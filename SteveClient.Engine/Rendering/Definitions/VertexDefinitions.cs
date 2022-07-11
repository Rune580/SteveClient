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
    public static VertexDefinition<Position> PositionTriangles;
    public static VertexDefinition<Position> Lines;

    public static void Init()
    {
        PositionColorTriangles = new VertexDefinition<PositionColor>(PrimitiveType.Triangles);
        PositionTextureColorTriangles = new VertexDefinition<PositionTextureColor>(PrimitiveType.Triangles);
        PositionTextureTriangles = new VertexDefinition<PositionTexture>(PrimitiveType.Triangles);
        DefaultFont = new VertexDefinition<PositionTexture>(PrimitiveType.Triangles);
        PositionTriangles = new VertexDefinition<Position>(PrimitiveType.Triangles);
        Lines = new VertexDefinition<Position>(PrimitiveType.Lines);
    }
    
    public readonly struct VertexDefinition<TVertex> where TVertex : IVertex
    {
        public readonly PrimitiveType PrimitiveType;
        public readonly int VertexSize;

        public VertexDefinition(PrimitiveType primitiveType) : this()
        {
            PrimitiveType = primitiveType;

            TVertex vertex = (TVertex)Activator.CreateInstance(VertexType)!;
            VertexSize = vertex.GetSize();
        }

        public Type VertexType => typeof(TVertex);
    }
}