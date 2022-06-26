using Ionic.Zlib;
using SteveClient.Engine.Networking.Exceptions;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound;

public abstract class ClientBoundPacket : IClientBoundPacket
{
    public int PacketId { get; private set; }
    
    public abstract void Read(in InPacketBuffer packetBuffer);

    public static void FromPacketBuffer(InPacketBuffer packetBuffer)
    {
        if (MinecraftNetworkingClient.Instance!.Connection!.CompressionThreshold > -1)
        {
            int dataLength = packetBuffer.ReadVarInt();

            if (dataLength > 0)
            {
                using MemoryStream packetStream = new MemoryStream(packetBuffer.ReadRest());
                using ZlibStream zlibStream = new ZlibStream(packetStream, CompressionMode.Decompress);

                byte[] buffer = new byte[dataLength];
                int bytesRead = zlibStream.Read(buffer, 0, dataLength);

                if (bytesRead != dataLength)
                    throw new InvalidPacketSizeException();

                packetBuffer = new InPacketBuffer(buffer);
            }
        }
        
        int packetId = packetBuffer.ReadVarInt();
        Type packetType = typeof(UnknownPacket);

        bool packetExists = PacketRegistry.TryGetClientBoundPacketRegistryEntry(packetId, out var registryEntry);
        if (packetExists)
            packetType = registryEntry.PacketType;

        ClientBoundPacket packet = (ClientBoundPacket)Activator.CreateInstance(packetType)!;
        packet.PacketId = packetId;
        
        packet.Read(packetBuffer);

        if (packetExists)
            registryEntry.PacketReceived?.Invoke(packet);
    }
}