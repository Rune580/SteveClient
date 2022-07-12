using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.ShaderDefinitions;
using static SteveClient.Engine.Rendering.Definitions.VertexDefinitions;

namespace SteveClient.Engine.Rendering.Definitions;

public static class RenderLayerDefinitions
{
    public static readonly List<BaseRenderLayer> Instances = new();

    public static readonly ChunkRenderLayer OpaqueBlockLayer = new(SolidBlockShader);

    public static readonly DefaultRenderLayer<PositionColor> PositionColorRenderLayer = new(PositionColorTriangles, PositionColorShader, TargetSpace.WorldSpace);
    // public static readonly DefaultRenderLayer<PositionTextureAtlas> SolidBlockLayer = new(PositionTextureAtlasTriangles, SolidBlockShader, TargetSpace.WorldSpace);
    public static readonly FontRenderLayer WorldFontLayer = new(TargetSpace.WorldSpace);
    public static readonly LineRenderLayer DebugLinesLayer = new();

    public static readonly DefaultRenderLayer<Position> UiColorRenderLayer = new(PositionTriangles, UiColorShader, TargetSpace.ScreenSpace);
    public static readonly FontRenderLayer ScreenFontLayer = new(TargetSpace.ScreenSpace);
    
    public static bool Rendering { get; private set; }
    
    public static void RebuildAll()
    {
        foreach (var renderLayer in Instances)
            renderLayer.RebuildBuffers();
    }

    public static void Render()
    {
        Rendering = true;
        
        foreach (var renderLayer in Instances)
        {
            renderLayer.Bind();
            renderLayer.PreRender();
            renderLayer.Render();
            renderLayer.PostRender();
        }

        Rendering = false;
    }
    
    public static void FlushAll()
    {
        foreach (var renderLayer in Instances)
            renderLayer.Flush();
    }
}