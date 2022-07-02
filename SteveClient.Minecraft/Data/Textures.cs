namespace SteveClient.Minecraft.Data;

public static class Textures
{
    private static readonly Dictionary<string, string> TextureMap = new();

    public static void Add(string texturePath)
    {
        TextureMap[GetResourceName(texturePath)] = texturePath;
    }

    public static string GetPath(string resourceName)
    {
        resourceName = resourceName.Replace("minecraft:", "");

        return TextureMap[resourceName];
    }

    public static KeyValuePair<string, string>[] GetTextures()
    {
        return TextureMap.ToArray();
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