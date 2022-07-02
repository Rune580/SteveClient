using SteveClient.Engine.Rendering;

namespace SteveClient.Engine.AssetManagement;

public static class TextureRegistry
{
    private static readonly Dictionary<string, Texture> Textures = new();

    public static void Add(KeyValuePair<string, string>[] textures)
    {
        foreach (var (resourceName, path) in textures)
        {
            Texture texture = new Texture(path);

            Textures[resourceName] = texture;
        }
    }
}