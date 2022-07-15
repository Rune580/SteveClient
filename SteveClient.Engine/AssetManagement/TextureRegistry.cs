using SteveClient.Engine.Rendering;
using SteveClient.Engine.Rendering.Textures;

namespace SteveClient.Engine.AssetManagement;

public static class TextureRegistry
{
    private static readonly Dictionary<string, Texture> Textures = new();
    
    public static TextureAtlas BlockTextureAtlas { get; private set; }

    public static void InitBlockTextureAtlas()
    {
        var textureCollection = Minecraft.Data.Textures.GetTextureCollection();

        BlockTextureAtlas = new TextureAtlas(16, 16, textureCollection.Length);
        
        foreach (var (resourceName, rawTexture) in textureCollection)
            BlockTextureAtlas.AddTexture(resourceName, rawTexture);
    }

    public static Texture GetTexture(string resourceName)
    {
        return Textures[resourceName];
    }
}