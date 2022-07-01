namespace SteveClient.Minecraft.DataGen.Parsers;

public class BlockTexturesParser : IMinecraftAssetParser
{
    public string JarPath => "assets/minecraft/textures/block/";
    public string LocalPath => "textures/block/";
    
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