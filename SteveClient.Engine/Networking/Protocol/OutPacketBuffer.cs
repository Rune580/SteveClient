using System.Text;
using OpenTK.Mathematics;
using SteveClient.Engine.Networking.Protocol.Utils;

namespace SteveClient.Engine.Networking.Protocol;

public class OutPacketBuffer
{
    protected readonly List<byte> ByteBuffer = new();

    public OutPacketBuffer() { }
    
    public OutPacketBuffer(IEnumerable<byte> bytes)
    {
        ByteBuffer.AddRange(bytes);
    }

    public OutPacketBuffer(OutPacketBuffer buffer)
    {
        ByteBuffer.AddRange(buffer.ByteBuffer);
    }

    public void WriteVector3d(Vector3d value)
    {
        WriteDouble(value.X);
        WriteDouble(value.Y);
        WriteDouble(value.Z);
    }

    public void WriteDouble(double value)
    {
        ByteBuffer.AddRange(BitConverter.GetBytes(value).Reverse());
    }

    public void WriteByteArray(byte[] bytes)
    {
        ByteBuffer.AddRange(bytes);
    }
    
    public void WriteByteArrayWithLength(byte[] bytes)
    {
        WriteVarInt(bytes.Length);
        ByteBuffer.AddRange(bytes);
    }

    public void WriteSignedByte(sbyte value)
    {
        ByteBuffer.Add((byte)value);
    }

    public void WriteLong(long value)
    {
        ByteBuffer.AddRange(BitConverter.GetBytes(value).Reverse());
    }

    public void WriteVarInt(int value)
    {
        while ((value & 128) != 0)
        {
            ByteBuffer.Add((byte) (value & 127 | 128));
            value = (int) ((uint) value) >> 7;
        }
        
        ByteBuffer.Add((byte) value);
    }

    public void WriteBool(bool value)
    {
        ByteBuffer.Add(value ? (byte)1 : (byte)0);
    }

    public void WriteString(string text)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        WriteByteArrayWithLength(bytes);
    }

    public void WriteUnsignedShort(ushort value)
    {
        ByteBuffer.AddRange(BitConverter.GetBytes(value));
    }

    public void WriteEnum<TEnum>(TEnum value) where TEnum : Enum
    {
        WriteVarInt(Convert.ToInt32(value));
    }

    public void WriteEnum(int value)
    {
        WriteVarInt(value);
    }

    public byte[] Flush()
    {
        byte[] bytes = ByteBuffer.ToArray();

        Clear();

        return bytes;
    }

    public void Clear()
    {
        ByteBuffer.Clear();
    }

    public int Length => ByteBuffer.Count;
}