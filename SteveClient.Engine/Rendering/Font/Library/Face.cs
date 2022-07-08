using FreeTypeSharp;
using FreeTypeSharp.Native;

namespace SteveClient.Engine.Rendering.Font.Library;

public unsafe class Face
{
    private FreeTypeLibrary _library;
    private IntPtr _face;
    public FT_FaceRec* Native { get; }

    public Face(FreeTypeLibrary library, string fontPath)
    {
        _library = library;

        if (FT.FT_New_Face(_library.Native, fontPath, 0, out _face) != 0)
            throw new Exception("Failed to load font!");

        Native = (FT_FaceRec*)_face;
    }

    public void SetPixelSizes(uint width, uint height)
    {
        FT.FT_Set_Pixel_Sizes(_face, width, height);
    }

    public void LoadChar(uint charCode)
    {
        if (FT.FT_Load_Char(_face, charCode, FT.FT_LOAD_RENDER) != 0)
            throw new Exception("Failed to load Glyph!");
    }
    
    public GlyphSlot Glyph => new(Native);

    public int FontHeight => Native->ascender - Native->descender;
    public short Height => Native->height;
}