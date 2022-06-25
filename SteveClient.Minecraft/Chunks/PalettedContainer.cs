using SteveClient.Minecraft.Chunks.Palettes;

namespace SteveClient.Minecraft.Chunks;

public class PalettedContainer
{
    protected byte BitsPerEntry;
    protected IPalette Palette;
    protected long[]? Data;

    public PalettedContainer(byte bitsPerEntry, int singleValue)
    {
        BitsPerEntry = bitsPerEntry;
        Palette = new SingleValuePalette(singleValue);
    }

    public PalettedContainer(byte bitsPerEntry, int[] palette, long[] data)
    {
        BitsPerEntry = bitsPerEntry;
        Palette = new IndirectPalette(palette);
        Data = data;
    }
    
    
}