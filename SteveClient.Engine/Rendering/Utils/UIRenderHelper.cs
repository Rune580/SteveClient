using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Utils;

public static class UiRenderHelper
{
    public static void Quad(Box2 rect, Color4 color, float depth = -100)
    {
        float[] vertices = new VertexDataArray(Position.Size * 4)
            .WithVector3(new Vector3(1, 1, 0))
            .WithVector3(new Vector3(1, 0, 0))
            .WithVector3(new Vector3(0, 0, 0))
            .WithVector3(new Vector3(0, 1, 0));

        uint[] indices = {
            0, 1, 3,
            1, 2, 3
        };

        Vector2 screenSize = WindowState.ScreenSize / 2;
        Vector3 pos = new Vector3(rect.Min.X - screenSize.X, screenSize.Y - (rect.Max.Y), depth);
        
        Matrix4 origin = Matrix4.CreateTranslation(pos);
        Matrix4 scale = Matrix4.CreateScale(new Vector3(rect.Max.X - rect.Min.X, rect.Max.Y - rect.Min.Y, 1));

        BakedQuad quad = new BakedQuad(vertices, indices, scale * origin, color);
        
        RenderLayerDefinitions.UiColorRenderLayer.UploadRenderData(quad);
    }
}