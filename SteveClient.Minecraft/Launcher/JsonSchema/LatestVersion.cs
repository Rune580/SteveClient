using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Launcher.JsonSchema;

public class LatestVersion
{
    [JsonPropertyName("release")]
    public string Release { get; set; }
    
    [JsonPropertyName("snapshot")]
    public string Snapshot { get; set; }
}