using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Launcher.JsonSchema;

public class MinecraftVersionLight
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
            
    [JsonPropertyName("type")]
    public string Type { get; set; }
            
    [JsonPropertyName("url")]
    public string Url { get; set; }
            
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }
            
    [JsonPropertyName("releaseTime")]
    public DateTime ReleaseTime { get; set; }
}

public static class MinecraftVersionLightExtensions
{
    public static string GetUrlByVersion(this MinecraftVersionLight[] versions, string version)
    {
        foreach (var versionLight in versions)
        {
            if (string.Equals(versionLight.Id, version, StringComparison.InvariantCultureIgnoreCase))
                return versionLight.Url;
        }

        throw new Exception($"Version {version} not found in manifest!");
    }
}