using OpenTK.Mathematics;
using SmartNbt.Tags;
using SteveClient.Engine.Networking.Protocol;
using SteveClient.Minecraft;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class ChunkDataAndUpdateLightPacket : ClientBoundPacket
{
    public Chunk Chunk;
    
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Vector2i chunkPos = packetBuffer.ReadChunkPos();
        NbtCompound heightMaps = packetBuffer.ReadNbtCompound();

        ChunkSection[] chunkSections = LoadChunkSections(new InPacketBuffer(packetBuffer.ReadByteArray()));

        Chunk = new Chunk(chunkPos, chunkSections);
    }

    private ChunkSection[] LoadChunkSections(InPacketBuffer dataBuffer)
    {
        ChunkSection[] chunkSections = new ChunkSection[Chunk.ChunkSectionCount];
        
        for (int i = 0; i < Chunk.ChunkSectionCount; i++)
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
                ulong[] dataArray = dataBuffer.ReadUnsignedLongArray();

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

        return chunkSections;
    }
}