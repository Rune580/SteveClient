namespace SteveClient.Minecraft.Chunks.Palettes;

public class IndirectPalette : IPalette
{
    private readonly int[] _palette;

    public IndirectPalette(int[] palette)
    {
        _palette = palette;
    }
    
    public int this[int i] => _palette[i];
}