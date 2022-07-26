using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Serilog;
using SteveClient.Minecraft.Chunks;

namespace SteveClient.Engine.Game;

public class LightMap
{ 
    private readonly int _skyLightTex;
    private readonly int _blockLightTex;

    private readonly bool[] _activeSections;
    private readonly ConcurrentDictionary<Vector3i, int> _sectionPosMap;
    
    public readonly int Width;
    public readonly int Height;

    public LightMap()
    {
        GL.CreateTextures(TextureTarget.Texture3D, 1, out _skyLightTex);
        GL.CreateTextures(TextureTarget.Texture3D, 1, out _blockLightTex);

        Width = (ClientSettings.RenderDistance + 1) * 8;
        Height = Chunk.ChunkSectionCount;
        
        InitLightMap(_skyLightTex, Width, Height);
        InitLightMap(_blockLightTex, Width, Height);

        _activeSections = new bool[Width * Width * Height];
        _sectionPosMap = new ConcurrentDictionary<Vector3i, int>();
    }

    public void Use(int baseUnit)
    {
        GL.BindTextureUnit(baseUnit, _skyLightTex);
        GL.BindTextureUnit(baseUnit + 1, _blockLightTex);
    }

    public void ReserveChunkSection(Vector3i sectionPos)
    {
        if (_sectionPosMap.TryGetValue(sectionPos, out var pos))
        {
            if (!_activeSections[pos])
                pos = GetNextFreeSection();
        }
        else
        {
            pos = GetNextFreeSection();
        }
        
        _activeSections[pos] = true;
        _sectionPosMap[sectionPos] = pos;
    }

    public void FreeChunkSection(Vector3i sectionPos)
    {
        int pos = _sectionPosMap[sectionPos];
        _activeSections[pos] = false;
        _sectionPosMap.TryRemove(sectionPos, out _);
    }

    public bool ContainsChunkSection(Vector3i sectionPos)
    {
        return _sectionPosMap.TryGetValue(sectionPos, out int pos) && _activeSections[pos];
    }

    public void Clear()
    {
        for (int i = 0; i < _activeSections.Length; i++)
            _activeSections[i] = false;
        
        _sectionPosMap.Clear();
    }

    public void UploadLightData(Vector3i sectionPos, ChunkSection section)
    {
        if (!_sectionPosMap.TryGetValue(sectionPos, out int pos))
            throw new KeyNotFoundException();

        Vector3i lightMapPos = ConvertLightMapPos(pos);

        if (lightMapPos.X < 0 || lightMapPos.Y < 0 || lightMapPos.Z < 0)
        {
            Console.WriteLine("sex");
        }

        if (lightMapPos.X > Width * 16|| lightMapPos.Y > Height * 16|| lightMapPos.Z > Width * 16)
        {
            Console.WriteLine("ligma");
        }

        GL.TextureSubImage3D(_skyLightTex, 0, lightMapPos.X, lightMapPos.Z, lightMapPos.Y, 16, 16, 16, PixelFormat.Red, PixelType.Float, section.GetSkyLights());
        GL.TextureSubImage3D(_blockLightTex, 0, lightMapPos.X, lightMapPos.Z, lightMapPos.Y, 16, 16, 16, PixelFormat.Red, PixelType.Float, section.GetBlockLights());
        
        Log.Verbose("Uploaded light data from ChunkSection {ChunkSectionPos}", sectionPos);
    }
    
    public Vector3i GetLightMapPos(Vector3i sectionPos)
    {
        return ConvertLightMapPos(_sectionPosMap[sectionPos]);
    }

    public int EncodeBlockPosOnLightMap(Vector3i sectionPos, Vector3i localPos)
    {
        Vector3i lightMapPos = GetLightMapPos(sectionPos);

        // lightMapPos.X += 16;
        // if (lightMapPos.X > 32)
        // {
        //     lightMapPos.X = 0;
        //     lightMapPos.Z += 16;
        // }

        int width = Width * 16;
        int depth = width * width;

        int x = lightMapPos.X + localPos.X;
        int z = (width * localPos.Z) + (lightMapPos.Z * width);
        int y = (depth * localPos.Y) + (lightMapPos.Y * depth);

        return x + y + z;
    }

    private int GetNextFreeSection()
    {
        for (int i = 0; i < _activeSections.Length; i++)
        {
            if (!_activeSections[i])
                return i;
        }

        throw new OutOfMemoryException();
    }

    private Vector3i ConvertLightMapPos(int pos)
    {
        float depth = Width * Width;

        int y = (int)MathF.Floor(pos / depth);
        int z = (int)MathF.Floor((pos - (y * depth)) / Width);
        int x = (int)MathF.Floor(pos - ((y * depth) + (z * Width)));

        return new Vector3i(x, y, z) * 16;
    }

    private static void InitLightMap(int lightMapTex, int renderDist, int sectionCount)
    {
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        
        GL.TextureStorage3D(lightMapTex, 1, SizedInternalFormat.R8, renderDist * 16, renderDist * 16, sectionCount * 16);
    }
}