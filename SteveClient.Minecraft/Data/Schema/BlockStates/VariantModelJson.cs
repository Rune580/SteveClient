using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Data.Schema.BlockStates;

public class VariantModelJson
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("x")]
    public int? X { get; set; }
    
    [JsonPropertyName("y")]
    public int? Y { get; set; }
    
    [JsonPropertyName("uvlock")]
    public bool? UvLock { get; set; }
    
    [JsonPropertyName("weight")]
    public int? Weight { get; set; }
}

public class VariantModelsJson
{
    public VariantModelJson[]? VariantModels { get; set; }
    public VariantModelJson? VariantModel { get; set; }
}