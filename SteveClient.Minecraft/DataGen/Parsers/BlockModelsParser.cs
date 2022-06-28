namespace SteveClient.Minecraft.DataGen.Parsers;

public class BlockModelsParser : IMinecraftAssetParser
{
    public string JarPath => "assets/minecraft/models/block/";
    public string LocalPath => "models/block/";
    
    public bool DataExists()
    {
        var path = Path.GetFullPath(LocalPath);
        
        bool dirExists = Directory.Exists(path);
        bool dirContainsFiles = false;

        if (dirExists)
            dirContainsFiles = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Any();
        
        return dirContainsFiles;
    }

    public void Parse()
    {
        
    }
}