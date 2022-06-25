namespace SteveClient.Engine.Networking.Protocol.Utils;

public static class ByteBufferExtensions
{
    public static byte[] Reverse(this byte[] bytes)
    {
        Array.Reverse(bytes);

        return bytes;
    }
}