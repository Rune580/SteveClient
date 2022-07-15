using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Data.Schema.Models;

public class ModelJson
{
    [JsonPropertyName("parent"), ]
    public string? Parent { get; set; }
    
    [JsonPropertyName("textures")]
    public Dictionary<string, string>? Textures { get; set; }

    [JsonPropertyName("elements")]
    public ElementJson[]? Elements { get; set; }
}