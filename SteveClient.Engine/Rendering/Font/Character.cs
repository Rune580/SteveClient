using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Font.Library;

namespace SteveClient.Engine.Rendering.Font;

public class Character
{
    public readonly int Handle;
    public readonly Vector2 Size;
    public readonly Vector2 Bearing;
    public readonly int Advance;

    public Character(GlyphSlot glyph, FtBitmap bitmap)
    {
        Handle = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, Handle);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0, PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);
        
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

        Size = new Vector2(bitmap.Width, bitmap.Rows);
        Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop);
        Advance = glyph.Advance.X;
    }

    public void Bind()
    {
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }
}