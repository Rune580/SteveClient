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

        return ref Data.Blocks.GetBlockStateFromBlockStateId(blockStateId);
    }

    public int GetBlockStateId(Vector3i pos)
    {
        int sectionIndex = GetSectionIndex(pos.Y);
        return _chunkSections[sectionIndex].GetBlockState(pos.X, HeightLocalToSection(pos.Y, sectionIndex), pos.Z);
    }

    public void SetBlockState(Vector3i pos, int blockStateId)
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
        return (int)MathF.Floor((float)(MaxBlockHeight - (height + NegativeHeight)) / ChunkSectionCount);
    }

    private int HeightLocalToSection(int height, int section)
    {
        return (height + NegativeHeight) - (section * 16);
    }
}