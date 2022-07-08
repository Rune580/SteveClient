using FreeTypeSharp.Native;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Font.Library;

public unsafe class GlyphSlot
{
    public FT_GlyphSlotRec* Native { get; }

    public int BitmapLeft => Native->bitmap_left;
    public int BitmapTop => Native->bitmap_top;
    public Vector2i Advance => new((int)Native->advance.x, (int)Native->advance.y);
    public FtBitmap Bitmap => new(Native->bitmap);

    public int Height => (int)Native->metrics.height;

    public GlyphSlot(FT_FaceRec* faceRec)
    {
        Native = faceRec->glyph;

        if (FT.FT_Render_Glyph((IntPtr)Native, FT_Render_Mode.FT_RENDER_MODE_SDF) != 0)
            throw new Exception("Failed to render glyph as SDF!");
    }
}