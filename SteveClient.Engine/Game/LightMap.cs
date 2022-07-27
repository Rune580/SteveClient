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

    private readonly ConcurrentDictionary<Vector3i, bool> _activeSections;
    private readonly ConcurrentDictionary<Vector3i, Vector3i> _sectionLightPosMap;
    
    public readonly int Width;
    public readonly int Height;

    public LightMap()
    {
        GL.CreateTextures(TextureTarget.Texture3D, 1, out _skyLightTex);
        GL.CreateTextures(TextureTarget.Texture3D, 1, out _blockLightTex);

        Width = (ClientSettings.RenderDistance + 1) * 2;
        Height = Chunk.ChunkSectionCount;
        
        InitLightMap(_skyLightTex, Width, Height);
        InitLightMap(_blockLightTex, Width, Height);

        _activeSections = new ConcurrentDictionary<Vector3i, bool>();
        
        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _activeSections[new Vector3i(x, y, z)] = false;
                }
            }
        }
        
        _sectionLightPosMap = new ConcurrentDictionary<Vector3i, Vector3i>();
    }

    public void Use(int baseUnit)
    {
        GL.BindTextureUnit(baseUnit, _skyLightTex);
        GL.BindTextureUnit(baseUnit + 1, _blockLightTex);
    }

    public void ReserveChunkSection(Vector3i sectionPos)
    {
        if (_sectionLightPosMap.TryGetValue(sectionPos, out var lightMapPos))
        {
            if (!_activeSections[lightMapPos])
                lightMapPos = GetNextFreeSection();
        }
        else
        {
            lightMapPos = GetNextFreeSection();
        }
        
        _activeSections[lightMapPos] = true;
        _sectionLightPosMap[sectionPos] = lightMapPos;
    }

    public void FreeChunkSection(Vector3i sectionPos)
    {
        Vector3i lightPos = _sectionLightPosMap[sectionPos];
        
        _activeSections[lightPos] = false;
        _sectionLightPosMap.TryRemove(sectionPos, out _);
    }

    public bool ContainsChunkSection(Vector3i sectionPos)
    {
        return _sectionLightPosMap.TryGetValue(sectionPos, out Vector3i lightPos) && _activeSections[lightPos];
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _activeSections[new Vector3i(x, y, z)] = false;
                }
            }
        }
        
        _sectionLightPosMap.Clear();
    }

    public void UploadLightData(Vector3i sectionPos, ChunkSection section)
    {
        ReserveChunkSection(sectionPos);
        
        if (!_sectionLightPosMap.TryGetValue(sectionPos, out Vector3i lightMapPos))
            throw new KeyNotFoundException();

        Vector3i lightMapTexPos = lightMapPos * 16;

        GL.TextureSubImage3D(_skyLightTex, 0, lightMapTexPos.X, lightMapTexPos.Z, lightMapTexPos.Y, 16, 16, 16, PixelFormat.Red, PixelType.UnsignedByte, section.GetSkyLights());
        GL.TextureSubImage3D(_blockLightTex, 0, lightMapTexPos.X, lightMapTexPos.Z, lightMapTexPos.Y, 16, 16, 16, PixelFormat.Red, PixelType.UnsignedByte, section.GetBlockLights());
        
        Log.Verbose("Uploaded light data from ChunkSection {ChunkSectionPos}, From: {TexFrom}, To: {TexTo}", sectionPos, lightMapTexPos, lightMapTexPos + new Vector3i(16));
    }
    
    public Vector3i GetLightMapPos(Vector3i sectionPos)
    {
        return _sectionLightPosMap[sectionPos] * 16;
    }

    public int EncodeBlockPosOnLightMap(Vector3i sectionPos, Vector3i localPos)
    {
        Vector3i lightMapPos = GetLightMapPos(sectionPos);

        int x = lightMapPos.X + localPos.X;
        int y = lightMapPos.Y + localPos.Y;
        int z = lightMapPos.Z + localPos.Z;

        int widthMask = 0xFFF >> 1;
        int heightMask = widthMask >> 1;

        return ((x & widthMask) << 22) | ((z & widthMask) << 11) | (y & heightMask);
    }

    private Vector3i GetNextFreeSection()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int z = 0; z < Width; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3i section = new Vector3i(x, y, z);

                    if (!_activeSections[section])
                        return section;
                }
            }
        }

        throw new OutOfMemoryException();
    }

    private static void InitLightMap(int lightMapTex, int renderDist, int sectionCount)
    {
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        GL.TextureParameter(lightMapTex, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        
        GL.TextureStorage3D(lightMapTex, 1, SizedInternalFormat.R16, renderDist * 16, renderDist * 16, sectionCount * 16);
    }
}