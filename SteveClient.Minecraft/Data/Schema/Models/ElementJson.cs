using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data.Schema.JsonConverters;
using SteveClient.Minecraft.ModelLoading;

namespace SteveClient.Minecraft.Data.Schema.Models;

public class ElementJson
{
    [JsonPropertyName("from"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 From { get; set; }
    
    [JsonPropertyName("to"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 To { get; set; }

    [JsonPropertyName("rotation")]
    public RotationJson? Rotation { get; set; }

    [JsonPropertyName("faces")]
    public FacesJson? Faces { get; set; }

    public void ApplyRotation(in List<RawBlockFace> faces)
    {
        if (Rotation is null)
            return;

        Matrix3 rot = Rotation.AsMatrix();
        Vector3 origin = Rotation.Origin / 16f;

        foreach (var face in faces)
        {
            face.TopLeft = (rot * (face.TopLeft - origin)) + origin;
            face.TopRight = (rot * (face.TopRight - origin)) + origin;
            face.BottomLeft = (rot * (face.BottomLeft - origin)) + origin;
            face.BottomRight = (rot * (face.BottomRight - origin)) + origin;
        }
    }
}