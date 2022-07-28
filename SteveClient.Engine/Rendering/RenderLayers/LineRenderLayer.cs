using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.ShaderDefinitions;
using static SteveClient.Engine.Rendering.Definitions.VertexDefinitions;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class LineRenderLayer : DefaultRenderLayer<Position>
{
    public LineRenderLayer() : base(Lines, LineShader, TargetSpace.WorldSpace) { }
}