namespace SteveClient.Minecraft.Chunks.Palettes;

public class SingleValuePalette : IPalette
{
    private readonly int _globalPaletteId;

    public SingleValuePalette(int globalPaletteId)
    {
        _globalPaletteId = globalPaletteId;
    }
    
    public int this[int _] => _globalPaletteId;
}