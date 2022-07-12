using OpenTK.Graphics.OpenGL4;
using SkiaSharp;

namespace SteveClient.Engine.Rendering.Textures;

public class TextureAtlas : AbstractTexture
{
    private readonly Dictionary<string, int> _atlas;
    private readonly SKSurface[] _surfaces;
    private readonly int _width;
    private readonly int _height;

    private int _layers;
    
    public TextureAtlas(int width, int height, int layers)
    {
        _atlas = new Dictionary<string, int>();
        _surfaces = new SKSurface[layers];
        
        GL.CreateTextures(TextureTarget.Texture2DArray, 1, out int handle);

        GL.TextureParameter(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        
        GL.TextureStorage3D(handle, 1, SizedInternalFormat.Rgba8, width, height, layers);

        _width = width;
        _height = height;
        _layers = 0;
        Handle = handle;
    }

    public void AddImage(string key, string path, bool throwIfSizeDoesntMatch = false)
    {
        if (_layers >= _surfaces.Length)
            throw new Exception();

        var surface = LoadImage(path);

        var image = surface.PeekPixels();
        int width = image.Width;
        int height = image.Height;
        image.Dispose();
        
        if (width != _width || height != _height)
        {
            if (throwIfSizeDoesntMatch)
                throw new Exception();
        }

        _surfaces[_layers] = surface;

        image = surface.PeekPixels().WithColorType(SKColorType.Bgra8888);
        
        GL.TextureSubImage3D(Handle,
            0,
            0,
            0,
            _layers,
            _width,
            _height,
            1,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            image.GetPixels());


        _atlas[key] = _layers;
        _layers++;
    }

    public int GetAtlasLayer(string key)
    {
        return _atlas[key.Replace("minecraft:", "")];
    }
    
    public override void Dispose()
    {
        foreach (var surface in _surfaces)
            surface.Dispose();
    }
}