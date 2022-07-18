using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Data.Collections;

namespace SteveClient.Minecraft.DataGen.Parsers;

public class BlockTexturesParser : IMinecraftAssetParser
{
    public string JarPath => "assets/minecraft/textures/block/";
    public string LocalPath => "textures/block/";

    private string ExtraPath => "textures/block-extra/";

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
        string[] files = Directory.GetFiles(LocalPath, "*.png", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string mcmeta = $"{file}.mcmeta";

            if (File.Exists(mcmeta))
            {
                Textures.AddAnimatedTexture(file, mcmeta);
            }
            else
            {
                Textures.AddTexture(file);
            }
        }

        // files = Directory.GetFiles(ExtraPath, "*_n.png", SearchOption.AllDirectories);
        // foreach (var file in files)
        // {
        //     string mcmeta = $"{file}.mcmeta";
        //
        //     if (File.Exists(mcmeta))
        //     {
        //         Textures.BlockNormalCollection.AddAnimatedTexture(file, mcmeta);
        //     }
        //     else
        //     {
        //         Textures.BlockNormalCollection.AddTexture(file);
        //     }
        // }
    }
}