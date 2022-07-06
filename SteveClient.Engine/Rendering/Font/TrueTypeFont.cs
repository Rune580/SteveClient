using FreeTypeSharp;
using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.Font.Library;

namespace SteveClient.Engine.Rendering.Font;

public class TrueTypeFont
{
    private const uint AsciiMax = 128;

    private readonly Dictionary<char, Character> _characters = new();

    public TrueTypeFont(string font)
    {
        string fontPath = ParsePath(font);

        FreeTypeLibrary lib = new FreeTypeLibrary();

        Face face = new Face(lib, fontPath);

        face.SetPixelSizes(0, 48);
        
        GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

        for (uint c = 0; c < AsciiMax; c++)
        {
            try
            {
                face.LoadChar(c);
                GlyphSlot glyph = face.Glyph;
                FtBitmap bitmap = glyph.Bitmap;

                Character character = new Character(glyph, bitmap);

                _characters[(char)c] = character;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
    
    public Character this[char c] => _characters[c];

    private static string ParsePath(string font)
    {
        string[] files = Directory.GetFiles("fonts/", "*.ttf", SearchOption.AllDirectories);

        foreach (var fontFile in files)
        {
            string name = Path.GetFileNameWithoutExtension(fontFile);

            if (string.Equals(name, font, StringComparison.InvariantCultureIgnoreCase))
                return fontFile;
        }

        throw new FileNotFoundException();
    }
}