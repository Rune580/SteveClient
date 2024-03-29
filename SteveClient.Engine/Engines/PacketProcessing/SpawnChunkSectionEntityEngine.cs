﻿using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking.Packets.ClientBound.Play;
using SteveClient.Minecraft.Chunks;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.PacketProcessing;

public class SpawnChunkSectionEntityEngine : PacketProcessingEngine<ChunkDataAndUpdateLightPacket>
{
    private readonly IEntityFactory _entityFactory;
    private readonly World _world;
    
    public SpawnChunkSectionEntityEngine(IEntityFactory entityFactory, World world)
    {
        _entityFactory = entityFactory;
        _world = world;
    }
    
    protected override void Execute(float delta, ConsumablePacket<ChunkDataAndUpdateLightPacket> consumablePacket)
    {
        SpawnChunkSectionEntities(consumablePacket);
        
        consumablePacket.MarkConsumed();
    }

    private void SpawnChunkSectionEntities(ChunkDataAndUpdateLightPacket packet)
    {
        var chunkPos = packet.Chunk.Position;
        
        for (int i = 0; i < Chunk.ChunkSectionCount; i++)
        {
            uint id = Egid.NextId;

            EntityInitializer initializer =
                _entityFactory.BuildEntity<ChunkSectionDescriptor>(id, GameGroups.ChunkSections.BuildGroup);
            
            initializer.Init(new TransformComponent(new Vector3(chunkPos.X, 16 * (i - 4), chunkPos.Y)));
            initializer.Init(new ChunkSectionComponent(chunkPos, i, true));
        }

        _world.LoadChunk(packet.Chunk);
    }
}