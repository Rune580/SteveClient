using FreeTypeSharp;
using FreeTypeSharp.Native;

namespace SteveClient.Engine.Rendering.Font.Library;

public unsafe class Face
{
    private FreeTypeLibrary _library;
    private IntPtr _face;
    private FT_FaceRec* _faceRec;

    public Face(FreeTypeLibrary library, string fontPath)
    {
        _library = library;

        if (FT.FT_New_Face(_library.Native, fontPath, 0, out _face) != 0)
            throw new Exception("Failed to load font!");

        _faceRec = (FT_FaceRec*)_face;
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

    public GlyphSlot Glyph => new(_faceRec);
}