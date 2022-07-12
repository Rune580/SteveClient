using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace SteveClient.Engine.Rendering.Textures;

public abstract class AbstractTexture : IDisposable
{
    public int Handle { get; protected init; }

    public void Use(int unit = 0)
    {
        GL.BindTextureUnit(unit, Handle);
    }

    public abstract void Dispose();

    protected static SKSurface LoadImage(string path)
    {
        using var image = SKBitmap.Decode(path);
        var surface = SKSurface.Create(image.Info);
        using var canvas = surface.Canvas;
        
        canvas.Scale(1, -1, 0, 16 / 2.0f);
        canvas.DrawBitmap(image, 0, 0);
        canvas.Flush();

        return surface;
    }
}