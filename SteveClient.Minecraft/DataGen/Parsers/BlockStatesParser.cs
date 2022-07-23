using System.Text.Json;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.Data.Schema.BlockStates;

namespace SteveClient.Minecraft.DataGen.Parsers;

public class BlockStatesParser : IMinecraftAssetParser
{
    public string JarPath => "assets/minecraft/blockstates";
    public string LocalPath => "blockstates/";

    public bool DataExists()
    {
        var path = Path.GetFullPath(LocalPath);
        
        bool dirExists = Directory.Exists(path);
        bool dirContainsFiles = false;

        if (dirExists)
            dirContainsFiles = Directory.EnumerateFiles(path, "*.json", SearchOption.AllDirectories).Any();
        
        return dirContainsFiles;
    }

    public void Parse()
    {
        string[] files = Directory.GetFiles(LocalPath, "*.json", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var blockState = JsonSerializer.Deserialize<BlockStateJson>(File.ReadAllText(file)) ?? throw new InvalidOperationException();

            string resourceName = Path.GetFileNameWithoutExtension(file);
            
            BlockStateModels.Add(resourceName, blockState);
        }
    }
}