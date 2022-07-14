using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing;

public class BlockUpdateChunkSectionEngine : PacketProcessingEngine<BlockUpdatePacket>
{
    private readonly World _world;

    public BlockUpdateChunkSectionEngine(World world)
    {
        _world = world;
    }

    protected override void Execute(float delta, ConsumablePacket<BlockUpdatePacket> consumablePacket)
    {
        BlockUpdatePacket packet = consumablePacket.Get();

        Vector3i chunkSectionPos = World.ChunkSectionPosFromBlockPos(packet.BlockPos);
        
        Vector2i chunkPos = new Vector2i(chunkSectionPos.X, chunkSectionPos.Z);
        int sectionIndex = chunkSectionPos.Y;

        if (!_world.IsChunkLoaded(chunkPos))
        {
            consumablePacket.MarkConsumed();
            return;
        }
        
        _world.SetBlockStateId(packet.BlockPos, packet.BlockStateId);
        
        foreach (var ((chunkSections, count), _) in entitiesDB.QueryEntities<ChunkSectionComponent>(GameGroups.ChunkSections.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var chunkSectionComp = ref chunkSections[i];
                
                if(chunkSectionComp.ChunkPos != chunkPos)
                    continue;
        
                if (chunkSectionComp.SectionIndex != sectionIndex)
                    continue;
        
                chunkSectionComp.ShouldRender = true;
                
                consumablePacket.MarkConsumed();
                return;
            }
        }
    }
}