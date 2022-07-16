using System.Collections;
using System.Text.Json;
using SkiaSharp;
using SteveClient.Minecraft.Data.Schema.Textures;

namespace SteveClient.Minecraft.Data.Collections;

public class TextureCollection
{
    private readonly Dictionary<string, RawTexture> RawTextures = new();
    
    public int Frames { get; private set; }

    public int Length => RawTextures.Count;

    public RawTexture this[string resourceName]
    {
        get => RawTextures[resourceName];
        set => RawTextures[resourceName] = value;
    }

    public void AddTexture(string texturePath)
    {
        string resourceName = GetResourceName(texturePath);
        
        this[resourceName] = new RawTexture
        {
            TexturePath = texturePath,
            Frames = 1
        };

        Frames += this[resourceName].Frames;
    }

    public void AddAnimatedTexture(string texturePath, string mcmetaPath)
    {
        string resourceName = GetResourceName(texturePath);
        
        using var image = SKBitmap.Decode(texturePath);
        int frames = image.Height / 16;
        
        this[resourceName] = new RawTexture
        {
            TexturePath = texturePath,
            Frames = frames,
            McMetaJson = JsonSerializer.Deserialize<TextureMcMetaJson>(File.ReadAllText(mcmetaPath))
        };
        
        Frames += this[resourceName].Frames;
    }

    public Dictionary<string, RawTexture>.Enumerator GetEnumerator()
    {
        return RawTextures.GetEnumerator();
    }

    public class RawTexture
    {
        public string TexturePath = "";
        public int Frames;
        public TextureMcMetaJson? McMetaJson;
    }
    
    private static string GetResourceName(string path)
    {
        string textureName = Path.GetFileNameWithoutExtension(path);
        string localPath = Path.GetRelativePath(Directory.GetCurrentDirectory(), path);
        
        string[] paths = localPath.Replace("\\", "/").Split('/');

        string resourcePath = String.Join('/', paths[1..^1]);

        return $"{resourcePath}/{textureName}";
    }
}