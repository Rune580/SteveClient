using System.Text.Json;
using System.Text.Json.Serialization;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Data.Schema.JsonConverters;

public class DirectionsJsonConverter : JsonConverter<Directions>
{
    public override Directions Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string text = reader.GetString()!;

        // Switch statement because Enum.Parse is too slow.
        return text switch
        {
            "bottom" => Directions.Down,
            "down" => Directions.Down,
            "up" => Directions.Up,
            "north" => Directions.North,
            "south" => Directions.South,
            "west" => Directions.West,
            "east" => Directions.East,
            _ => Directions.None
        };
    }

    public override void Write(Utf8JsonWriter writer, Directions value, JsonSerializerOptions options)
    {
        
    }
}