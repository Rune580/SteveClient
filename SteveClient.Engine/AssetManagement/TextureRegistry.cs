using SteveClient.Engine.Rendering;
using SteveClient.Engine.Rendering.Textures;

namespace SteveClient.Engine.AssetManagement;

public static class TextureRegistry
{
    private static readonly Dictionary<string, Texture> Textures = new();
    
    public static TextureAtlas BlockTextureAtlas { get; private set; }

    public static void InitBlockTextureAtlas(KeyValuePair<string, string>[] textures)
    {
        BlockTextureAtlas = new TextureAtlas(16, 16, textures.Length);
        
        foreach (var (resourceName, path) in textures)
            BlockTextureAtlas.AddImage(resourceName, path);
    }

    public static Texture GetTexture(string resourceName)
    {
        return Textures[resourceName];
    }
}