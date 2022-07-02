using System.Text.Json;
using System.Text.Json.Serialization;
using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Data.JsonSchema.Converters;

public class Vector4JsonConverter : JsonConverter<Vector4>
{
    public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        float x = reader.GetSingle();

        reader.Read();
        float y = reader.GetSingle();

        reader.Read();
        float z = reader.GetSingle();

        reader.Read();
        float w = reader.GetSingle();
        reader.Read();

        return new Vector4(x, y, z, w);
    }

    public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}