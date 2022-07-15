using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data.Schema.JsonConverters;

namespace SteveClient.Minecraft.Data.Schema.Models;

public class ElementJson
{
    [JsonPropertyName("from"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 From { get; set; }
    
    [JsonPropertyName("to"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 To { get; set; }

    [JsonPropertyName("faces")]
    public FacesJson? Faces { get; set; }
}