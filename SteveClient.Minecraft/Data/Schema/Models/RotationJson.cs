using System.Text.Json.Serialization;
using OpenTK.Mathematics;
using SteveClient.Minecraft.Data.Schema.JsonConverters;

namespace SteveClient.Minecraft.Data.Schema.Models;

public class RotationJson
{
    [JsonPropertyName("angle")]
    public float Angle { get; set; }
    
    [JsonPropertyName("axis")]
    public string Axis { get; set; }
    
    [JsonPropertyName("origin"), JsonConverter(typeof(Vector3JsonConverter))]
    public Vector3 Origin { get; set; }

    public Matrix3 AsMatrix()
    {
        float angle = -(Angle * (MathF.PI / 180f));
        Matrix3 rotation = Matrix3.CreateFromAxisAngle(GetAxis(), angle);

        return rotation;
    }

    private Vector3 GetAxis()
    {
        if (string.Equals(Axis, "x", StringComparison.InvariantCultureIgnoreCase))
            return Vector3.UnitX;
        
        if (string.Equals(Axis, "y", StringComparison.InvariantCultureIgnoreCase))
            return Vector3.UnitY;
        
        if (string.Equals(Axis, "z", StringComparison.InvariantCultureIgnoreCase))
            return Vector3.UnitZ;

        throw new Exception();
    }
}