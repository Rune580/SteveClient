using SteveClient.Engine.Rendering.Textures;

namespace SteveClient.Engine.AssetManagement;

public static class TextureRegistry
{
    private static readonly Dictionary<string, Texture> Textures = new();
    
    public static TextureAtlas BlockTextureAtlas { get; private set; }
    public static TextureAtlas BlockNormalAtlas { get; private set; }

    public static void Init()
    {
        InitBlockTextureAtlas();
        InitBlockNormalAtlas();
    }

    private static void InitBlockTextureAtlas()
    {
        var texturesCollection = Minecraft.Data.Textures.GetBlockTextures();

        BlockTextureAtlas = new TextureAtlas(16, 16, texturesCollection.Length);
        
        foreach (var (resourceName, rawTexture) in texturesCollection)
            BlockTextureAtlas.AddTexture(resourceName, rawTexture);
    }

    private static void InitBlockNormalAtlas()
    {
        var normalsCollection = Minecraft.Data.Textures.GetBlockNormals();

        BlockNormalAtlas = new TextureAtlas(128, 128, normalsCollection.Length);

        foreach (var (resourceName, rawTexture) in normalsCollection)
            BlockNormalAtlas.AddTexture(resourceName, rawTexture);
    }

    public static Texture GetTexture(string resourceName)
    {
        return Textures[resourceName];
    }
}