using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Data.JsonSchema;

public class ModelJson
{
    [JsonPropertyName("parent"), ]
    public string? Parent { get; set; }
    
    [JsonPropertyName("textures")]
    public Dictionary<string, string>? Textures { get; set; }

    [JsonPropertyName("elements")]
    public ElementJson[]? Elements { get; set; }
}