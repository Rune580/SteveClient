using SteveClient.Minecraft.Collections;

namespace SteveClient.Minecraft.Data;

public static class Textures
{
    internal static readonly TextureCollection BlockTextureCollection = new();
    internal static readonly TextureCollection BlockNormalCollection = new();
    
    public static void AddTexture(string texturePath)
    {
        BlockTextureCollection.AddTexture(texturePath);
    }

    public static void AddAnimatedTexture(string texturePath, string mcmetaPath)
    {
        BlockTextureCollection.AddAnimatedTexture(texturePath, mcmetaPath);
    }

    public static TextureCollection GetBlockTextures()
    {
        return BlockTextureCollection;
    }
}