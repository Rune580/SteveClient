using OpenTK.Mathematics;
using SmartNbt.Tags;
using SteveClient.Engine.Networking.Protocol;
using SteveClient.Minecraft;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class ChunkDataAndUpdateLightPacket : ClientBoundPacket
{
    private const int ChunkSectionCount = MinecraftDefinition.WorldHeight / 16;
    
    
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Vector2i chunkPos = packetBuffer.ReadChunkPos();
        NbtCompound heightMaps = packetBuffer.ReadNbtCompound();
        
        InPacketBuffer dataBuffer = new InPacketBuffer(packetBuffer.ReadByteArray());
        
        ChunkSection[] chunkSections = new ChunkSection[ChunkSectionCount];

        for (int i = 0; i < ChunkSectionCount; i++)
        {
            short blockCount = dataBuffer.ReadShort();

            int bitsPerEntry = dataBuffer.ReadUnsignedByte();
            if (bitsPerEntry == 0)
            {
                chunkSections[i] = new ChunkSection(blockCount, dataBuffer.ReadVarInt());
                dataBuffer.ReadVarInt();
            }
            else
            {
                int[] palette = dataBuffer.ReadVarIntArray();
                long[] dataArray = dataBuffer.ReadLongArray();

                chunkSections[i] = new ChunkSection(blockCount, bitsPerEntry, palette, dataArray);
            }

            bitsPerEntry = dataBuffer.ReadUnsignedByte();
            if (bitsPerEntry == 0)
            {
                dataBuffer.ReadVarInt();
                dataBuffer.ReadVarInt();
            }
            else
            {
                int[] palette = dataBuffer.ReadVarIntArray();
                long[] dataArray = dataBuffer.ReadLongArray();
            }
        }
    }
}