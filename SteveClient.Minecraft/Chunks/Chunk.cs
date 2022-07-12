using OpenTK.Mathematics;
using SteveClient.Minecraft.Blocks;

namespace SteveClient.Minecraft.Chunks;

public class Chunk
{
    public const int ChunkSectionCount = MinecraftDefinition.WorldHeight / 16;
    public const int MaxBlockHeight = ChunkSectionCount * 16;
    private const int NegativeHeight = 64;
    
    public Vector2i Position { get; }
    private readonly ChunkSection[] _chunkSections;

    public Chunk(Vector2i position, ChunkSection[] chunkSections)
    {
        Position = position;
        _chunkSections = chunkSections;
    }

    public ref readonly BlockState GetBlockState(Vector3i pos)
    {
        int sectionIndex = GetSectionIndex(pos.Y);
        int blockStateId = _chunkSections[sectionIndex].GetBlockState(pos.X, HeightLocalToSection(pos.Y, sectionIndex), pos.Z);

        return ref Data.Blocks.GetBlockState(blockStateId);
    }

    public int GetBlockStateId(Vector3i pos)
    {
        int sectionIndex = GetSectionIndex(pos.Y);

        if (sectionIndex is < 0 or >= 20)
            return -1;
        
        return _chunkSections[sectionIndex].GetBlockState(pos.X, HeightLocalToSection(pos.Y, sectionIndex), pos.Z);
    }

    public void SetBlockState(Vector3i pos, short blockStateId)
    {
        int sectionIndex = GetSectionIndex(pos.Y);
        _chunkSections[sectionIndex].SetBlockState(pos.X, HeightLocalToSection(pos.Y, sectionIndex), pos.Z, blockStateId);
    }

    public ChunkSection GetChunkSection(int sectionIndex)
    {
        return _chunkSections[sectionIndex];
    }

    private int GetSectionIndex(int height)
    {
        return (int)MathF.Floor((height + NegativeHeight) / 16f);
    }

    private int HeightLocalToSection(int height, int section)
    {
        return height - (section * 16 - NegativeHeight);
    }
}