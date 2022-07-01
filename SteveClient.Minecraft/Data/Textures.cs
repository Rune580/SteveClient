namespace SteveClient.Minecraft.Data;

public static class Textures
{
    private static readonly Dictionary<string, string> TextureMap = new();

    public static void Add(string texturePath)
    {
        string resourceName = Path.GetFileNameWithoutExtension(texturePath);

        TextureMap[resourceName] = texturePath;
    }

    public static string GetPath(string resourceName)
    {
        resourceName = resourceName.Replace("minecraft:", "");

        return TextureMap[resourceName];
    }
}