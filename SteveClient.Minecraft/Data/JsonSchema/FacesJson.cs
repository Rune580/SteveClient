using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data.JsonSchema.Converters;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Data.JsonSchema;

public class FacesJson
{
    [JsonPropertyName("down")]
    public FaceJson? Down { get; set; }
    
    [JsonPropertyName("up")]
    public FaceJson? Up { get; set; }
    
    [JsonPropertyName("north")]
    public FaceJson? North { get; set; }
    
    [JsonPropertyName("south")]
    public FaceJson? South { get; set; }
    
    [JsonPropertyName("west")]
    public FaceJson? West { get; set; }
    
    [JsonPropertyName("east")]
    public FaceJson? East { get; set; }
    
    public class FaceJson
    {
        [JsonPropertyName("uv"), JsonConverter(typeof(Vector4JsonConverter))]
        public Vector4? Uv { get; set; }
        
        [JsonPropertyName("texture")]
        public string Texture { get; set; }
        
        [JsonPropertyName("rotation")]
        public int? Rotation { get; set; }
        
        [JsonPropertyName("cullface"), JsonConverter(typeof(DirectionsJsonConverter))]
        public Directions? CullFace { get; set; }
    }
}