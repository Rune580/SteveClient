using SteveClient.Minecraft.Data;

namespace SteveClient.Minecraft.Chunks;

public class ChunkSection
{
    public const int Height = 16;
    public const int Width = 16;

    public bool TrustEdges;
    
    private readonly List<int> _palette = new();
    private readonly short[] _blockStates;
    private readonly int[] _skyLights;
    private readonly int[] _blockLights;

    private short _blockCount;

    private ChunkSection()
    {
        _skyLights = new int[4096];
        _blockLights = new int[4096];
        
        Array.Fill(_skyLights, 0);
        Array.Fill(_blockLights, 0);
    }
    
    public ChunkSection(short blockCount, int singleValue) : this()
    {
        _blockCount = blockCount;
        
        _palette.Add(singleValue);
        
        _blockStates = new short[4096];
        Array.Fill(_blockStates, (short)singleValue);
    }
    
    public ChunkSection(short blockCount, int bitsPerEntry, int[] palette, ulong[] data) : this()
    {
        _blockCount = blockCount;
        
        _palette.AddRange(palette);

        _blockStates = new short[4096];

        PopulateBlockStates(bitsPerEntry, data);
    }

    public int GetBlockState(int x, int y, int z)
    {
        return _blockStates[GetBlockNumber(x, y, z)];
    }

    public void SetBlockState(int x, int y, int z, short blockStateId)
    {
        _blockStates[GetBlockNumber(x, y, z)] = blockStateId;
    }

    public void SetSkyLight(int x, int y, int z, int value) => _skyLights[GetBlockNumber(x, y, z)] = value;

    public int GetSkyLight(int x, int y, int z) => _skyLights[GetBlockNumber(x, y, z)];

    public int[] GetSkyLights() => _skyLights;

    public void SetBlockLight(int x, int y, int z, int value) => _blockLights[GetBlockNumber(x, y, z)] = value;

    public int GetBlockLight(int x, int y, int z) => _blockLights[GetBlockNumber(x, y, z)];

    public int[] GetBlockLights() => _blockLights;

    public void LoadSkyLightData(byte[] data)
    {
        int i = 0;
        
        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x+=2)
                {
                    byte value = data[i];
                    
                    SetSkyLight(x, y, z, (byte)(value & 0xF));
                    SetSkyLight(x + 1, y, z, (byte)((value >> 4) & 0xF));
                    
                    i++;
                }
            }
        }
    }

    public void LoadBlockLightData(byte[] data)
    {
        int i = 0;
        
        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x+=2)
                {
                    byte value = data[i];
                    
                    SetBlockLight(x, y, z, (byte)(value & 0xF));
                    SetBlockLight(x + 1, y, z, (byte)((value >> 4) & 0xF));
                    
                    i++;
                }
            }
        }
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

                    short blockStateId = (short)data;
                    
                    if (bitsPerEntry < Blocks.MaxBitsPerEntry)
                    {
                        if (blockStateId >= _palette.Count)
                            throw new IndexOutOfRangeException();
                        
                        blockStateId = (short)_palette[blockStateId];
                    }

                    SetBlockState(x, y, z, blockStateId);
                }
            }
        }
    }
    
    private static int GetBlockNumber(int x, int y, int z) => (y * Height + z) * Width + x;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is not ChunkSection chunkSection)
            return false;

        return this == chunkSection;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_palette, _blockStates, _blockCount);
    }

    public static bool operator ==(ChunkSection? left, ChunkSection? right)
    {
        if (left is null || right is null)
            return false;

        return left._blockCount == right._blockCount 
               && left._palette.SequenceEqual(right._palette)
               && left._blockStates.SequenceEqual(right._blockStates);
    }

    public static bool operator !=(ChunkSection? left, ChunkSection? right)
    {
        return !(left == right);
    }
}