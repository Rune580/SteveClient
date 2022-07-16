using OpenTK.Graphics.OpenGL4;
using SkiaSharp;
using SteveClient.Engine.Rendering.Textures.Atlas;
using SteveClient.Minecraft.Data.Collections;

namespace SteveClient.Engine.Rendering.Textures;

public class TextureAtlas : AbstractTexture
{
    private readonly Dictionary<string, IAtlasTexture> _atlas;
    private readonly SKSurface[] _surfaces;

    private int _layers;
    
    public TextureAtlas(int width, int height, int layers)
    {
        _atlas = new Dictionary<string, IAtlasTexture>();
        _surfaces = new SKSurface[layers];
        
        GL.CreateTextures(TextureTarget.Texture2DArray, 1, out int handle);

        GL.TextureParameter(handle, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(handle, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(handle, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        
        GL.TextureStorage3D(handle, 1, SizedInternalFormat.Rgba8, width, height, layers);

        Width = width;
        Height = height;
        _layers = 0;
        Handle = handle;
    }

    public void AddImage(string path, int frameOffset = 0, bool throwIfSizeDoesntMatch = false)
    {
        if (_layers >= _surfaces.Length)
            throw new Exception();

        var surface = LoadImage(path, frameOffset);

        var image = surface.PeekPixels();
        int width = image.Width;
        int height = image.Height;
        image.Dispose();
        
        if (width != Width || height != Height)
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
            Width,
            Height,
            1,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            image.GetPixels());
    }

    public void AddTexture(string key, TextureCollection.RawTexture rawTexture)
    {
        if (rawTexture.Frames > 1 && rawTexture.McMetaJson is not null)
        {
            SKSurface[] surfaces = new SKSurface[rawTexture.Frames];

            for (int i = 0; i < rawTexture.Frames; i++)
                surfaces[i] = LoadImage(rawTexture.TexturePath, i);

            _atlas[key] = new AtlasAnimatedTexture(Handle, _layers, rawTexture.McMetaJson, surfaces);
        }
        else
        {
            AddImage(rawTexture.TexturePath);
            _atlas[key] = new AtlasTexture(_layers);
        }

        _layers++;
    }

    public int GetAtlasLayer(string key)
    {
        return _atlas[key.Replace("minecraft:", "")].GetAtlasLayer();
    }

    public bool ContainsKey(string key)
    {
        return _atlas.ContainsKey(key);
    }

    public string[] GetKeys()
    {
        return _atlas.Keys.ToArray();
    }
    
    public override void Dispose()
    {
        foreach (var surface in _surfaces)
            surface.Dispose();
    }
}