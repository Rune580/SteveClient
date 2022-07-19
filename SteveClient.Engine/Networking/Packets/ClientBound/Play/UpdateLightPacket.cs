using OpenTK.Mathematics;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Protocol;
using SteveClient.Minecraft.Chunks;
using SteveClient.Minecraft.Collections;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class UpdateLightPacket : ClientBoundPacket
{
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Vector2i chunkPos = packetBuffer.ReadChunkPos();
        Chunk chunk = World.ServerWorld!.GetChunk(chunkPos);

        bool trustEdges = packetBuffer.ReadBool();

        for (int i = 0; i < Chunk.ChunkSectionCount; i++)
            chunk.GetChunkSection(i).TrustEdges = trustEdges;
        
        BitSet skyLightMask = packetBuffer.ReadBitSet();
        BitSet blockLightMask = packetBuffer.ReadBitSet();
        BitSet emptySkyLightMask = packetBuffer.ReadBitSet();
        BitSet emptyBlockLightMask = packetBuffer.ReadBitSet();
        
        int skyLightArrayCount = packetBuffer.ReadVarInt();
        for (int i = 0; i < Chunk.ChunkSectionCount + 2; i++)
        {
            if (emptySkyLightMask[i])
            {
                chunk.GetChunkSection(i).LoadSkyLightData(new byte[2048]);
                continue;
            }
            
            if (!skyLightMask[i])
                continue;
            
            var data = packetBuffer.ReadByteArray();
            
            if (i is 0 or 21)
                continue;
            
            chunk.GetChunkSection(i).LoadSkyLightData(data);
        }

        int blockLightArrayCount = packetBuffer.ReadVarInt();
        for (int i = 0; i < Chunk.ChunkSectionCount + 2; i++)
        {
            if (emptyBlockLightMask[i])
            {
                chunk.GetChunkSection(i).LoadBlockLightData(new byte[2048]);
                continue;
            }
            
            if (!blockLightMask[i])
                continue;
            
            var data = packetBuffer.ReadByteArray();
            
            if (i is 0 or 21)
                continue;
            
            chunk.GetChunkSection(i).LoadBlockLightData(data);
        }
    }
}