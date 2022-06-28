namespace SteveClient.Minecraft.DataGen.Parsers;

public interface IMinecraftAssetParser
{
    string JarPath { get; }
    string LocalPath { get; }
    
    bool DataExists();
    void Parse();
}

public static class MinecraftAssetParserExtensions
{
    public static bool DataExists(this IMinecraftAssetParser[] assetParsers)
    {
        return assetParsers.All(assetParser => assetParser.DataExists());
    }
}