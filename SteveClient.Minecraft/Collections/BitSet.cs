namespace SteveClient.Minecraft.Collections;

public class BitSet
{
    private const int LongMask = 0x3f;
    
    private long[] _bits;

    public BitSet(long[] bits)
    {
        _bits = bits;
    }
    
    public BitSet(int nBits)
    {
        if (nBits < 0)
            throw new ArgumentOutOfRangeException(nameof(nBits));
 
        uint length = (uint)nBits >> 6;
        if ((nBits & LongMask) != 0)
            length++;
        _bits = new long[length];
    }

    public bool this[int pos] => Get(pos);

    public void Clear()
    {
        Array.Fill(_bits, 0);
    }
    
    public void Clear(int pos)
    {
        int offset = pos >> 6;
        Ensure(offset);
        _bits[offset] &= ~(1L << pos);
    }

    public bool Get(int pos)
    {
        int offset = pos >> 6;
        
        if (offset >= _bits.Length)
            return false;
        
        return (_bits[offset] & (1L << pos)) != 0;
    }

    public void Set(int pos)
    {
        int offset = pos >> 6;
        Ensure(offset);
        _bits[offset] |= 1L << pos;
    }

    public void Set(int index, bool value)
    {
        if (value)
        {
            Set(index);
        }
        else
        {
            Clear(index);
        }
    }
    
    private void Ensure(int lastElt)
    {
        if (lastElt >= _bits.Length)
        {
            long[] nd = new long[lastElt + 1];
            Array.Copy(_bits, 0, nd, 0, _bits.Length);
            _bits = nd;
        }
    }
}