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
    
    public ChunkSection(short blockCount, int bitsPerEntry, int[] palette, long[] data)
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

    public ref readonly BlockState this[Vector3i pos] => ref Data.Blocks.GetBlockStateFromBlockStateId(_palette[_blockStates[pos.Y][pos.Z][pos.X]]);

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

                    _blockStates[y][z][x] = (byte)data;
                }
            }
        }
    }
}