using OpenTK.Mathematics;
using SmartNbt.Tags;
using SteveClient.Engine.Networking.Protocol;

namespace SteveClient.Engine.Networking.Packets.ClientBound.Play;

public class ChunkDataAndUpdateLightPacket : ClientBoundPacket
{
    public override void Read(in InPacketBuffer packetBuffer)
    {
        Vector2i chunkPos = packetBuffer.ReadChunkPos();
        NbtCompound heightMaps = packetBuffer.ReadNbtCompound();
        
    }
}