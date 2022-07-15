using OpenTK.Graphics.OpenGL4;
using SkiaSharp;
using SteveClient.Engine.Rendering.Definitions;

namespace SteveClient.Engine.Rendering.Utils;

public static class TextureInterpolator
{
    private static int _inputTextureArray;
    private static int _outputTexture;
    
    public static int GetOutputTexture() => _outputTexture;
    
    public static void Init()
    {
        GL.CreateTextures(TextureTarget.Texture2DArray, 1, out _inputTextureArray);
        GL.TextureParameter(_inputTextureArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(_inputTextureArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(_inputTextureArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(_inputTextureArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TextureStorage3D(_inputTextureArray, 1, SizedInternalFormat.Rgba8, 16, 16, 2);
        
        GL.CreateTextures(TextureTarget.Texture2D, 1, out _outputTexture);
        GL.TextureParameter(_outputTexture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(_outputTexture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(_outputTexture, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(_outputTexture, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TextureStorage2D(_outputTexture, 1, SizedInternalFormat.Rgba8, 16, 16);
    }

    public static void Interpolate(SKPixmap tex1, SKPixmap tex2, float weight, int handle = 1, int atlasLevel = 0)
    {
        GL.BindImageTexture(0, _outputTexture, 0, false, 0, TextureAccess.ReadWrite, SizedInternalFormat.Rgba8);
        
        CopyImageIntoTexArray(tex1, 0);
        CopyImageIntoTexArray(tex2, 1);
        
        var shader = ShaderDefinitions.TextureInterpolatorShader;
        shader.Use();
        
        GL.BindTextureUnit(1, _inputTextureArray);
        GL.BindTextureUnit(0, _outputTexture);

        shader.SetFloat("weight", weight);
        shader.SetInt("textures", 1);
        shader.SetInt("interpolatedTexture", 0);
        
        GL.DispatchCompute((int)MathF.Ceiling(16 / 8f), (int)MathF.Ceiling(16 / 4f), 1);
        GL.MemoryBarrier(MemoryBarrierFlags.AllBarrierBits);
        
        GL.BindImageTexture(0, _inputTextureArray, 0, false, atlasLevel, TextureAccess.ReadWrite, SizedInternalFormat.Rgba8);
        
        GL.CopyImageSubData(_outputTexture, ImageTarget.Texture2D, 0, 0, 0, 0,
            handle, ImageTarget.Texture2DArray, 0, 0, 0, atlasLevel, 16, 16, 1);
    }

    private static void CopyImageIntoTexArray(SKPixmap image, int layer)
    {
        GL.TextureSubImage3D(_inputTextureArray,
            0,
            0,
            0,
            layer,
            16,
            16,
            1,
            PixelFormat.Bgra,
            PixelType.UnsignedByte,
            image.GetPixels());
    }
}