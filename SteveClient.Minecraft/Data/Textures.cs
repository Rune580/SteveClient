using SteveClient.Minecraft.Data.Collections;

namespace SteveClient.Minecraft.Data;

public static class Textures
{
    private static readonly TextureCollection TextureCollection = new();
    
    public static void AddTexture(string texturePath)
    {
        TextureCollection.AddTexture(GetResourceName(texturePath), texturePath);
    }

    public static void AddAnimatedTexture(string texturePath, string mcmetaPath)
    {
        TextureCollection.AddAnimatedTexture(GetResourceName(texturePath), texturePath, mcmetaPath);
    }

    public static TextureCollection GetTextureCollection()
    {
        return TextureCollection;
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