using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Definitions;

public static class RenderLayerDefinitions
{
    public static readonly List<BaseRenderLayer> Instances = new();

    public static readonly DefaultRenderLayer<PositionColor> PositionColorRenderLayer = new(VertexDefinitions.PositionColorTriangles);
}