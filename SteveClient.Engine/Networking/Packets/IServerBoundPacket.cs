using Ionic.Zlib;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets;

public interface IServerBoundPacket
{
    void Write(in OutPacketBuffer packetBuffer);

    void Send()
    {
        if (MinecraftNetworkingClient.Instance is null)
            throw new NullReferenceException("Instance cannot be null!");

        if (MinecraftNetworkingClient.Instance.Connection is null)
            throw new NullReferenceException("Connection cannot be null!");

        MinecraftNetworkingClient.Instance.Connection.EnqueuePacket(this);
    }

    byte[] Flush()
    {
        int packetId = PacketRegistry.ServerBoundPacketMap[GetType()];

        OutPacketBuffer packetDataBuffer = new OutPacketBuffer();
        packetDataBuffer.WriteVarInt(packetId);
        Write(packetDataBuffer);
        
        OutPacketBuffer packetBuffer = new OutPacketBuffer();

        int threshold = MinecraftNetworkingClient.Instance!.Connection!.CompressionThreshold;
        if (threshold > -1)
        {
            OutPacketBuffer subBuffer = new OutPacketBuffer();
            byte[] packetBytes = packetDataBuffer.Flush();

            if (packetBytes.Length >= threshold)
            {
                subBuffer.WriteVarInt(packetBytes.Length);

                using MemoryStream dataStream = new MemoryStream();
                using ZlibStream compressor = new ZlibStream(dataStream, CompressionMode.Compress);

                compressor.Write(packetBytes);
            }
            else
            {
                subBuffer.WriteVarInt(0);
                subBuffer.WriteByteArray(packetBytes);
                
                packetBuffer.WriteByteArrayWithLength(subBuffer.Flush());
            }
        }
        else
        {
            packetBuffer.WriteByteArrayWithLength(packetDataBuffer.Flush());
        }
        
        return packetBuffer.Flush();
    }
}