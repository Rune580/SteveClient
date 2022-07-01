using OpenTK.Mathematics;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Chunks.Palettes;

namespace SteveClient.Minecraft.Chunks;

public class ChunkSection
{
    private const int Height = 16;
    private const int Width = 16;
    
    private readonly List<int> _palette = new();
    private readonly byte[][][] _blockStates;

    private short _blockCount;
    
    public ChunkSection(short blockCount, int singleValue)
    {
        _blockCount = blockCount;
        
        _palette.Add(singleValue);

        byte[] x = new byte[16];
        Array.Fill(x, (byte)0);

        byte[][] z = new byte[16][];
        Array.Fill(z, x);
        
        _blockStates = new byte[16][][];
        Array.Fill(_blockStates, z);
    }
    
    public ChunkSection(short blockCount, int bitsPerEntry, int[] palette, ulong[] data)
    {
        _blockCount = blockCount;
        
        _palette.AddRange(palette);

        byte[] x = new byte[16];
        byte[][] z = new byte[16][];
        _blockStates = new byte[16][][];
        
        Array.Fill(z, x);
        Array.Fill(_blockStates, z);
        
        PopulateBlockStates(bitsPerEntry, data);
    }

    public int GetBlockState(int x, int y, int z)
    {
        return _palette[_blockStates[y][z][x]];
    }

    public void SetBlockState(int x, int y, int z, int blockStateId)
    {
        throw new NotImplementedException();
    }
    
    private void PopulateBlockStates(int bitsPerEntry, ulong[] dataArray)
    {
        int blocksPerArray = 64 / bitsPerEntry;
        uint valueMask = (uint)((1 << bitsPerEntry) - 1);

        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int blockNumber = (((y * Height) + z) * Width) + x;
                    int index = blockNumber / blocksPerArray;
                    int offset = (blockNumber * bitsPerEntry) % (blocksPerArray * bitsPerEntry);

                    uint data = (uint)(dataArray[index] >> offset);
                    data &= valueMask;

                    if (data >= _palette.Count)
                        throw new IndexOutOfRangeException();

                    _blockStates[y][z][x] = (byte)data;
                }
            }
        }
    }
}