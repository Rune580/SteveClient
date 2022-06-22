using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Definitions;

public static class VertexDefinitions
{
    public static VertexDefinition<PositionColor> PositionColorDefinition;
    public static VertexDefinition<PositionColor> DebugWireFrameDefinition;

    public static void Init()
    {
        PositionColorDefinition = new VertexDefinition<PositionColor>(ShaderDefinitions.PositionColorShader, PrimitiveType.Triangles);

        DebugWireFrameDefinition = new VertexDefinition<PositionColor>(ShaderDefinitions.PositionColorShader, PrimitiveType.Lines);
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