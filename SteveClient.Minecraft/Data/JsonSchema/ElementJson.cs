using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data.JsonSchema.Converters;

namespace SteveClient.Minecraft.Data.JsonSchema;

public class ElementJson
{
    [JsonPropertyName("from"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 From { get; set; }
    
    [JsonPropertyName("to"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 To { get; set; }

    [JsonPropertyName("faces")]
    public FacesJson? Faces { get; set; }
}