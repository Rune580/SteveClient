using OpenTK.Graphics.OpenGL4;
using SkiaSharp;
using SteveClient.Engine.Rendering.Utils;
using SteveClient.Minecraft.Data.Schema.Textures;

namespace SteveClient.Engine.Rendering.Textures.Atlas;

public class AtlasAnimatedTexture : IAtlasTexture, IDisposable
{
    private static readonly List<AtlasAnimatedTexture> Instances = new();

    public static void TickAnimations()
    {
        foreach (var instance in Instances)
            instance.Tick();
    }

    private readonly int _handle;
    private readonly SKSurface[] _surfaces;
    private readonly int _atlasLayer;
    private readonly int _ticksPerFrame;
    private readonly int[] _animFrames;
    private readonly bool _interpolate;

    private SKPixmap? _image;
    private int _ticks;
    private int _frame;
    
    public AtlasAnimatedTexture(int handle, int atlasLayer, TextureMcMetaJson mcMetaJson, SKSurface[] surfaces)
    {
        _handle = handle;
        _atlasLayer = atlasLayer;
        _surfaces = surfaces;

        var animation = mcMetaJson.Animation;

        if (animation.Frames is not null)
        {
            _animFrames = animation.Frames;
        }
        else
        {
            _animFrames = new int[surfaces.Length];

            for (int i = 0; i < surfaces.Length; i++)
                _animFrames[i] = i;
        }

        _ticksPerFrame = animation.FrameTime ?? 1;

        _interpolate = animation.Interpolate ?? false;
        
        _ticks = 0;
        _frame = 0;


        UpdateBuffer();
        Instances.Add(this);
    }

    public int GetAtlasLayer() => _atlasLayer;

    private void Tick()
    {
        _ticks++;

        if (_ticks < _ticksPerFrame)
        {
            if (_interpolate)
                InterpolateFrame();
            
            return;
        }
        
        _ticks -= _ticksPerFrame;
        NextFrame();
    }

    private void NextFrame()
    {
        _frame++;

        if (_frame >= _animFrames.Length)
            _frame = 0;

        if (!_interpolate)
            UpdateBuffer();
    }

    private void UpdateBuffer()
    {
        _image?.Dispose();
        
        _image = _surfaces[_animFrames[_frame]].PeekPixels().WithColorType(SKColorType.Bgra8888);
        
        GL.TextureSubImage3D(_handle,
            0,
            0,
            0,
            _atlasLayer,
            16,
            16,
            1,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            _image.GetPixels());
    }

    private void InterpolateFrame()
    {
        int nextFrame = _frame + 1;
        if (nextFrame >= _animFrames.Length)
            nextFrame = 0;
        
        using SKPixmap tex1 = _surfaces[_animFrames[_frame]].PeekPixels().WithColorType(SKColorType.Bgra8888);
        using SKPixmap tex2 = _surfaces[_animFrames[nextFrame]].PeekPixels().WithColorType(SKColorType.Bgra8888);
        
        float weight = (float)_ticks / _ticksPerFrame;

        TextureInterpolator.Interpolate(tex1, tex2, weight, _handle, _atlasLayer);
    }

    public void Dispose()
    {
        foreach (var surface in _surfaces)
            surface.Dispose();
    }
}