using OpenTK.Mathematics;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Chunks.Palettes;

namespace SteveClient.Minecraft.Chunks;

public class ChunkSection
{
    private const int Height = 16;
    private const int Width = 16;
    
    private readonly List<int> _palette = new();
    private readonly short[][][] _blockStates;

    private short _blockCount; 

    public ChunkSection(short blockCount, int singleValue)
    {
        _blockCount = blockCount;
        
        _palette.Add(singleValue);

        short[] x = new short[16];
        Array.Fill(x, (byte)0);

        short[][] z = new short[16][];
        Array.Fill(z, x);
        
        _blockStates = new short[16][][];
        Array.Fill(_blockStates, z);
    }
    
    public ChunkSection(short blockCount, int bitsPerEntry, int[] palette, long[] data)
    {
        _blockCount = blockCount;
        
        _palette.AddRange(palette);

        short[] x = new short[16];
        short[][] z = new short[16][];
        _blockStates = new short[16][][];
        
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
    
    private void PopulateBlockStates(int bitsPerEntry, long[] dataArray)
    {
        uint valueMask = (uint)((1 << bitsPerEntry) - 1);

        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int blockNumber = (((y * Height) + z) * Width) + x;
                    int startLong = (blockNumber * bitsPerEntry) / 64;
                    int startOffset = (blockNumber * bitsPerEntry) % 64;
                    int endLong = ((blockNumber + 1) * bitsPerEntry - 1) / 64;

                    uint data;
                    if (startLong == endLong) 
                    {
                        data = (uint)(dataArray[startLong] >> startOffset);
                    } 
                    else
                    {
                        int endOffset = 64 - startOffset;
                        data = (uint)(dataArray[startLong] >> startOffset | dataArray[endLong] << endOffset);
                    }

                    data &= valueMask;

                    _blockStates[y][z][x] = (short)data;
                }
            }
        }
    }
}