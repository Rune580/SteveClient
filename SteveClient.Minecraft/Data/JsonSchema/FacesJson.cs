using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Data.JsonSchema;

public class FacesJson
{
    [JsonPropertyName("down")]
    public FaceJson? Down { get; set; }
    
    [JsonPropertyName("Up")]
    public FaceJson? Up { get; set; }
    
    [JsonPropertyName("North")]
    public FaceJson? North { get; set; }
    
    [JsonPropertyName("South")]
    public FaceJson? South { get; set; }
    
    [JsonPropertyName("West")]
    public FaceJson? West { get; set; }
    
    [JsonPropertyName("East")]
    public FaceJson? East { get; set; }
    
    public class FaceJson
    {
        [JsonPropertyName("uv")]
        public Vector4 Uv { get; set; }
        
        [JsonPropertyName("texture")]
        public string Texture { get; set; }
        
        [JsonPropertyName("cullface")]
        public string CullFace { get; set; }
    }
}