using OpenTK.Mathematics;
using SmartNbt.Tags;
using SteveClient.Engine.Networking.Protocol;
using SteveClient.Minecraft;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Collections;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class ChunkDataAndUpdateLightPacket : ClientBoundPacket
{
    public Chunk Chunk;

    private bool _trustEdges;

    public override void Read(in InPacketBuffer packetBuffer)
    {
        Vector2i chunkPos = packetBuffer.ReadChunkPos();
        NbtCompound heightMaps = packetBuffer.ReadNbtCompound();

        ChunkSection[] chunkSections = LoadChunkSections(packetBuffer);

        foreach (var section in chunkSections)
            section.TrustEdges = _trustEdges;

        Chunk = new Chunk(chunkPos, chunkSections);
    }

    private ChunkSection[] LoadChunkSections(InPacketBuffer packetBuffer)
    {
        InPacketBuffer dataBuffer = new InPacketBuffer(packetBuffer.ReadByteArray());
        
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
                ulong[] dataArray = dataBuffer.ReadUnsignedLongArray();
            }
        }
        
        int blockEntityCount = packetBuffer.ReadVarInt();
        for (int i = 0; i < blockEntityCount; i++)
        {
            byte packedXy = packetBuffer.ReadUnsignedByte();
            short height = packetBuffer.ReadShort();
            int blockEntityType = packetBuffer.ReadVarInt();
            var data = packetBuffer.ReadNbtCompound();
        }

        _trustEdges = packetBuffer.ReadBool();

        BitSet skyLightMask = packetBuffer.ReadBitSet();
        BitSet blockLightMask = packetBuffer.ReadBitSet();
        BitSet emptySkyLightMask = packetBuffer.ReadBitSet();
        BitSet emptyBlockLightMask = packetBuffer.ReadBitSet();
        
        int skyLightArrayCount = packetBuffer.ReadVarInt();
        
        for (int i = 0; i < Chunk.ChunkSectionCount + 2; i++)
        {
            if (!skyLightMask[i])
                continue;
            
            var data = packetBuffer.ReadByteArray();
            
            if (i is 0 or 21)
                continue;
            
            chunkSections[i].LoadSkyLightData(data);
        }

        int blockLightArrayCount = packetBuffer.ReadVarInt();
        
        for (int i = 0; i < Chunk.ChunkSectionCount + 2; i++)
        {
            if (!blockLightMask[i])
                continue;
            
            var data = packetBuffer.ReadByteArray();
            
            if (i is 0 or 21)
                continue;
            
            chunkSections[i].LoadBlockLightData(data);
        }

        return chunkSections;
    }
    
}