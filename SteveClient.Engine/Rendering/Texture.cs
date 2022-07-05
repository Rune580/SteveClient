using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace SteveClient.Engine.Rendering;

public class Texture : IDisposable
{
    public readonly int Handle;
    private SKSurface? _surface;

    private Texture(int handle)
    {
        Handle = handle;
    }

    public Texture(string imagePath) : this(GL.GenTexture())
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, Handle);

        var image = LoadImageData(imagePath);
        
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
            PixelFormat.Bgra, PixelType.UnsignedByte, image.GetPixels());
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
    }
    
    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    private SKPixmap LoadImageData(string path)
    {
        _surface?.Dispose();

        using var image = SKBitmap.Decode(path);
        _surface = SKSurface.Create(image.Info);
        using var canvas = _surface.Canvas;
        
        canvas.Scale(1, -1, 0, image.Height / 2.0f);
        canvas.DrawBitmap(image, 0, 0);
        canvas.Flush();

        return _surface.PeekPixels().WithColorType(SKColorType.Bgra8888);
    }

    public void Dispose()
    {
        _surface?.Dispose();
    }
}