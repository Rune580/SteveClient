using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Data.JsonSchema;

public class ElementJson
{
    [JsonPropertyName("from")]
    public Vector3 From;
    
    [JsonPropertyName("to")]
    public Vector3 To;

    [JsonPropertyName("faces")]
    public FacesJson Faces;
}