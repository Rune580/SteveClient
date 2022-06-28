using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Launcher.JsonSchema;

public class VersionManifest
{
    [JsonPropertyName("latest")]
    public LatestVersion Latest { get; set; }
        
    [JsonPropertyName("versions")]
    public MinecraftVersionLight[] Versions { get; set; }
}