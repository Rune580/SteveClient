using FreeTypeSharp.Native;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Font.Library;

public unsafe class GlyphSlot
{
    private FT_GlyphSlotRec* _glyphRec;
    
    public int BitmapLeft { get; }
    public int BitmapTop { get; }
    public Vector2i Advance { get; }
    public FtBitmap Bitmap { get; }

    public GlyphSlot(FT_FaceRec* faceRec)
    {
        _glyphRec = faceRec->glyph;
        
        BitmapLeft = _glyphRec->bitmap_left;
        BitmapTop =_glyphRec->bitmap_top;
        Advance = new Vector2i((int)_glyphRec->advance.x, (int)_glyphRec->advance.y);
        
        if (FT.FT_Render_Glyph((IntPtr)_glyphRec, FT_Render_Mode.FT_RENDER_MODE_SDF) != 0)
            throw new Exception("Failed to render glyph as SDF!");

        Bitmap = new FtBitmap(_glyphRec->bitmap);
    }
}