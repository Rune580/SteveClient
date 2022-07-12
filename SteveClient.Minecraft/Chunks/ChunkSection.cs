namespace SteveClient.Minecraft.Chunks;

public class ChunkSection
{
    private const int Height = 16;
    private const int Width = 16;
    
    private readonly List<int> _palette = new();
    private readonly short[] _blockStates;

    private short _blockCount;
    
    public ChunkSection(short blockCount, int singleValue)
    {
        _blockCount = blockCount;
        
        _palette.Add(singleValue);
        
        _blockStates = new short[16 * 16 * 16];
        Array.Fill(_blockStates, (short)singleValue);
    }
    
    public ChunkSection(short blockCount, int bitsPerEntry, int[] palette, ulong[] data)
    {
        _blockCount = blockCount;
        
        _palette.AddRange(palette);

        _blockStates = new short[16 * 16 * 16];

        PopulateBlockStates(bitsPerEntry, data);
    }

    public int GetBlockState(int x, int y, int z)
    {
        int blockNumber = (((y * Height) + z) * Width) + x;
        return _blockStates[blockNumber];
    }

    public void SetBlockState(int x, int y, int z, short blockStateId)
    {
        int blockNumber = (((y * Height) + z) * Width) + x;
        _blockStates[blockNumber] = blockStateId;
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

                    SetBlockState(x, y, z, (short)_palette[(int)data]);
                }
            }
        }
    }

    public long GetContentHash()
    {
        // long hash;
        //
        // unchecked
        // {
        //     long paletteHash = _palette.Aggregate(_blockCount * (_palette.Count ^ 31), (total, next) => total ^ next);
        //
        //     hash = _blockStates.Aggregate(paletteHash, (yTotal, yNext) =>
        //     {
        //         var y = yNext.Aggregate(yTotal, (zTotal, zNext) =>
        //         {
        //             var z = zNext.Aggregate(zTotal, (xTotal, xNext) => xTotal * xNext);
        //
        //             return zTotal ^ z;
        //         });
        //
        //         return yTotal * y;
        //     });
        // }
        //
        // return hash;

        return 0;
    }

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