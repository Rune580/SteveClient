using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Utils;

public static class LineRenderer
{
    public static void DrawLine(Vector3 start, Vector3 end, Color4 color)
    {
        var vertexFactory = RenderLayerDefinitions.DebugLinesLayer.GetVertexFactory();
        
        IVertex[] vertices = vertexFactory.Consume(new[] { start, end }, Array.Empty<Vector3>(), Array.Empty<Color4>(), Array.Empty<Vector2>());
        
        RenderLayerDefinitions.DebugLinesLayer.UploadRenderData(new BakedLine(vertices.VertexData(), color));
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        DrawLine(start, end, Color4.White);
    }
}