using FreeTypeSharp.Native;

namespace SteveClient.Engine.Rendering.Font.Library;

public unsafe struct FtBitmap
{
    public int Width { get; }
    public int Rows { get; }
    public IntPtr Buffer { get; }

    public FtBitmap(FT_Bitmap bitmap)
    {
        Width = (int)bitmap.width;
        Rows = (int)bitmap.rows;
        Buffer = bitmap.buffer;
    }
}