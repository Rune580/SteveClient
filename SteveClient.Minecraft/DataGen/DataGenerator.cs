using System.IO.Compression;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.DataGen.Generators;
using SteveClient.Minecraft.DataGen.Parsers;
using SteveClient.Minecraft.Launcher;
using SteveClient.Minecraft.Utils;

namespace SteveClient.Minecraft.DataGen;

public static class DataGenerator
{
    private static readonly IMinecraftAssetParser[] AssetParsers =
    {
        new BlockTexturesParser(),
        new BlockModelsParser(),
        new BlockStatesParser()
    };
    
    public static void GenerateData()
    {
        BlocksGen.GenerateBlocks();
        ValidateMinecraftAssets();
        AssetParsers.Parse();
    }

    private static void ValidateMinecraftAssets()
    {
        if (AssetParsers.DataExists())
            return;
        
        DownloadAndExtractAssets(AssetParsers).ConfigureAwait(true).GetAwaiter().GetResult();
    }

    private static async Task DownloadAndExtractAssets(IMinecraftAssetParser[] assetParsers)
    {
        var package = await WebHelper.GetMinecraftVersionedPackage();
        await WebHelper.DownloadFileAsync(package.Downloads.Client.Url, "minecraft/client.jar");

        var jar = ZipFile.OpenRead("minecraft/client.jar");
        
        foreach (var assetParser in assetParsers)
        {
            if (assetParser.DataExists())
                continue;
            
            var path = Path.GetFullPath(assetParser.LocalPath);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            foreach (var entry in jar.Entries)
            {
                if (!entry.FullName.Contains(assetParser.JarPath))
                    continue;
                
                var extractPath = Path.Combine(assetParser.LocalPath, entry.Name);
                entry.ExtractToFile(extractPath, true);
            }
        }
        
        jar.Dispose();
    }
}