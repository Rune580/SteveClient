using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Definitions;

public static class RenderLayerDefinitions
{
    public static readonly List<BaseRenderLayer> Instances = new();

    public static readonly DefaultRenderLayer<PositionColor> PositionColorRenderLayer = new(VertexDefinitions.PositionColorTriangles);
    public static readonly DefaultRenderLayer<PositionTextureColor> PositionTextureColorLayer = new(VertexDefinitions.PositionTextureColorTriangles);
    public static readonly FontRenderLayer DefaultFontLayer = new();
    public static readonly LineRenderLayer DebugLinesLayer = new();
    
    public static readonly ScreenSpaceRenderLayer<PositionTexture> ScreenSpacePositionTextureLayer = new(VertexDefinitions.PositionTextureTriangles);

    public static void RebuildAll()
    {
        foreach (var renderLayer in Instances)
            renderLayer.RebuildBuffers();
    }
    
    public static void FlushAll()
    {
        foreach (var renderLayer in Instances)
            renderLayer.Flush();
    }
}