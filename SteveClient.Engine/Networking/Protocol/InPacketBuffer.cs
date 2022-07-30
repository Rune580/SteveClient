using System.Collections;
using System.Net.Sockets;
using System.Text;
using OpenTK.Mathematics;
using Serilog;
using Serilog.Core;
using SmartNbt;
using SmartNbt.Tags;
using SteveClient.Engine.Networking.Exceptions;
using SteveClient.Engine.Networking.Protocol.Utils;
using SteveClient.Minecraft.Collections;

namespace SteveClient.Engine.Networking.Protocol;

public class InPacketBuffer : Stream
{
    protected readonly byte[] ByteBuffer;
    protected int Offset;

    public InPacketBuffer(NetworkStream stream)
    {
        int packetSize = ReadVarIntFunc(() => (byte)stream.ReadByte());
        
        ByteBuffer = new byte[packetSize];
        int bytesRead = stream.Read(ByteBuffer, 0, ByteBuffer.Length);

        if (bytesRead != packetSize)
            throw new InvalidPacketSizeException();
    }

    public InPacketBuffer(byte[] buffer)
    {
        ByteBuffer = buffer;
    }

    public byte[] ReadRest()
    {
        int length = ByteBuffer.Length - Offset;
        return ReadByteArray(length);
    }

    public string[] ReadIdentifierArray()
    {
        int count = ReadVarInt();

        string[] identifiers = new string[count];

        for (int i = 0; i < count; i++)
            identifiers[i] = ReadString();

        return identifiers;
    }

    public NbtCompound ReadNbtCompound()
    {
        if (PeekNbtTag() == NbtTagType.End)
        {
            Offset++;
            return new NbtCompound();
        }

        NbtReader reader = new NbtReader(this);
        
        return (NbtCompound)reader.ReadAsTag();
    }

    public NbtTagType PeekNbtTag()
    {
        return (NbtTagType)ByteBuffer[Offset];
    }

    public NbtTag ReadNbtTag()
    {
        NbtReader reader = new NbtReader(this);

        return reader.ReadAsTag();
    }

    public Vector2i ReadChunkPosInt()
    {
        return new Vector2i(ReadInt(), ReadInt());
    }

    public Vector2i ReadChunkPosVarInt()
    {
        return new Vector2i(ReadVarInt(), ReadVarInt());
    }

    public Vector3d ReadVector3d()
    {
        return new Vector3d(ReadDouble(), ReadDouble(), ReadDouble());
    }

    public Vector3i ReadDelta()
    {
        short x = ReadShort();
        short y = ReadShort();
        short z = ReadShort();
        
        return new Vector3i(x, y, z);
    }

    public Vector3d ReadVelocity()
    {
        short x = ReadShort();
        short y = ReadShort();
        short z = ReadShort();

        return new Vector3d(x / 8000d, y / 8000d, z / 8000d);
    }

    public Vector3i ReadPosition()
    {
        ulong posEncodedLong = ReadUnsignedLong();

        int x = (int)(posEncodedLong >> 38);
        int y = (int)(posEncodedLong & 0xFFF);
        int z = (int)((posEncodedLong >> 12) & 0x3FFFFFF);

        if (x >= 1 << 25)
            x -= 1 << 26;

        if (y >= 1 << 11)
            y -= 1 << 12;

        if (z >= 1 << 25)
            z -= 1 << 26;

        return new Vector3i(x, y, z);
    }

    public BitSet ReadBitSet()
    {
        long[] data = ReadLongArray();

        return new BitSet(data);
    }

    public bool ReadBool()
    {
        return ReadUnsignedByte() == 1;
    }

    public string ReadString(int length)
    {
        byte[] bytes = ReadByteArray(length);

        return Encoding.UTF8.GetString(bytes);
    }

    public string ReadString()
    {
        return ReadString(ReadVarInt());
    }

    public float ReadFloat()
    {
        return BitConverter.ToSingle(ReadByteArray(4).Reverse());
    }

    public double ReadDouble()
    {
        return BitConverter.ToDouble(ReadByteArray(8).Reverse());
    }

    public short ReadShort()
    {
        return BitConverter.ToInt16(ReadByteArray(2).Reverse());
    }

    public int ReadInt()
    {
        return BitConverter.ToInt32(ReadByteArray(4).Reverse());
    }

    public long ReadLong()
    {
        return BitConverter.ToInt64(ReadByteArray(8).Reverse());
    }

    public ulong ReadUnsignedLong()
    {
        return BitConverter.ToUInt64(ReadByteArray(8).Reverse());
    }

    public long[] ReadLongArray()
    {
        long[] longArray = new long[ReadVarInt()];
        for (int i = 0; i < longArray.Length; i++)
            longArray[i] = ReadLong();
        return longArray;
    }

    public ulong[] ReadUnsignedLongArray()
    {
        ulong[] array = new ulong[ReadVarInt()];
        for (int i = 0; i < array.Length; i++)
            array[i] = ReadUnsignedLong();
        return array;
    }

    public int ReadVarInt()
    {
        return ReadVarIntFunc(ReadUnsignedByte);
    }

    public int[] ReadVarIntArray()
    {
        int[] intArray = new int[ReadVarInt()];
        for (int i = 0; i < intArray.Length; i++)
            intArray[i] = ReadVarInt();
        return intArray;
    }
    
    public byte ReadUnsignedByte()
    {
        if (Offset >= ByteBuffer.Length)
            throw new EndOfStreamException();

        byte b = ByteBuffer[Offset];
        Offset++;
        
        return b;
    }

    public sbyte ReadSignedByte()
    {
        return (sbyte)ReadUnsignedByte();
    }

    public byte[] ReadByteArray(int length)
    {
        int max = ByteBuffer.Length - Offset;
        byte[] bytes = new byte[length];
        
        if (length > max)
        {
            Log.Warning("Clamping length of byte array read from {Orig} to {New}", length, max);
            Array.Copy(ByteBuffer, Offset, bytes, 0, max);
            
            Array.Fill(bytes, (byte)0, max, length - max);
        }
        else
        {
            Array.Copy(ByteBuffer, Offset, bytes, 0, length);
        }
        Offset += length;
        return bytes;
    }

    public byte[] ReadByteArray()
    {
        return ReadByteArray(ReadVarInt());
    }

    private int ReadVarIntFunc(Func<byte> readFunc)
    {
        var value = 0;
        var size = 0;
        int b;
        while (((b = readFunc()) & 0x80) == 0x80)
        {
            value |= (b & 0x7F) << (size++*7);
            if (size > 5)
                throw new IOException("This VarInt is an imposter!");
        }
        return value | ((b & 0x7F) << (size*7));
    }
    
    public override void Flush()
    {
        throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int maxToRead = Math.Min(Math.Min(buffer.Length - offset, count), ByteBuffer.Length - Offset);
        Array.Copy(ByteBuffer, Offset, buffer, offset, maxToRead);
        Offset += maxToRead;
        return maxToRead;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException();
    }

    public override bool CanRead => true;
    public override bool CanSeek => false;
    public override bool CanWrite => false;
    public override long Length => ByteBuffer.Length;
    public override long Position
    {
        get => Offset;
        set => throw new NotSupportedException();
    }
}