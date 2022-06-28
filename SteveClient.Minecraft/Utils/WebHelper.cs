using System.Net.Http.Json;
using SteveClient.Minecraft.Launcher.JsonSchema;

namespace SteveClient.Minecraft.Utils;

public static class WebHelper
{
    private static HttpClient? _instance;
    private static HttpClient Client => _instance ??= new HttpClient();

    public static async Task DownloadFileAsync(string url, string path, string fileName)
    {
        path = Path.GetFullPath(path);
        
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        string destFile = Path.Join(path, fileName);

        if (File.Exists(destFile))
            File.Delete(destFile);

        await File.WriteAllBytesAsync(destFile, await Client.GetByteArrayAsync(url));
    }

    public static async Task DownloadFileAsync(string url, string filePath)
    {
        string[] strings = filePath.Replace("\\", "/").Split("/");

        await DownloadFileAsync(url, filePath.Replace(strings.Last(), ""), strings.Last());
    }

    public static void DownloadFile(string url, string filePath)
    {
        DownloadFileAsync(url, filePath).ConfigureAwait(true).GetAwaiter().GetResult();
    }

    public static async Task<MinecraftVersionedPackage> GetMinecraftVersionedPackage()
    {
        var versionManifest = await Client.GetFromJsonAsync<VersionManifest>("https://launchermeta.mojang.com/mc/game/version_manifest.json");
        if (versionManifest is null)
            throw new NullReferenceException();
        
        string packageUrl = versionManifest.Versions.GetUrlByVersion(MinecraftDefinition.Version);

        var package = await Client.GetFromJsonAsync<MinecraftVersionedPackage>(packageUrl);
        if (package is null)
            throw new NullReferenceException();

        return package;
    }
}