using System.Text.Json;
using SteveClient.Minecraft.Data.JsonSchema;

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
        string[] files = Directory.GetFiles(LocalPath, "*.json", SearchOption.AllDirectories);

        Dictionary<string, ModelJson> modelJsons = new Dictionary<string, ModelJson>();
        
        foreach (var file in files)
        {
            var model = JsonSerializer.Deserialize<ModelJson>(File.ReadAllText(file)) ?? throw new InvalidOperationException();

            if (model.Parent is not null)
            {
                model.Parent = model.Parent
                    .Replace("minecraft:", "")
                    .Replace("block/", "");
            }
            
            modelJsons[Path.GetFileNameWithoutExtension(file)] = model;
        }
    }
}