using System.Text.Json.Serialization;
using SteveClient.Minecraft.Data.Schema.JsonConverters;

namespace SteveClient.Minecraft.Data.Schema.BlockStates;

public class BlockStateJson
{
    [JsonPropertyName("variants"), JsonConverter(typeof(VariantsJsonConverter))]
    public Dictionary<string, VariantModelJson[]>? Variants { get; set; }
}