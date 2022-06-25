using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Definitions;

public static class VertexDefinitions
{
    public static VertexDefinition<PositionColor> PositionColorTriangles;
    public static VertexDefinition<PositionColor> PositionColorLines;

    public static void Init()
    {
        PositionColorTriangles = new VertexDefinition<PositionColor>(ShaderDefinitions.PositionColorShader, PrimitiveType.Triangles);

        PositionColorLines = new VertexDefinition<PositionColor>(ShaderDefinitions.PositionColorShader, PrimitiveType.Lines);
    }
    
    public readonly struct VertexDefinition<TVertex> where TVertex : IVertex
    {
        public readonly Shader Shader;
        public readonly PrimitiveType PrimitiveType;

        public VertexDefinition(Shader shader, PrimitiveType primitiveType)
        {
            Shader = shader;
            PrimitiveType = primitiveType;
        }

        public Type VertexType => typeof(TVertex);
    }
}