using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Data.Schema.Textures;

public class AnimationJson
{
    [JsonPropertyName("interpolate")]
    public bool? Interpolate { get; set; }
    
    [JsonPropertyName("frametime")]
    public int? FrameTime { get; set; }
    
    [JsonPropertyName("frames")]
    public int[]? Frames { get; set; }
}