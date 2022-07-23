using System.Text.Json.Serialization;
using SteveClient.Minecraft.BlockStructs;
using SteveClient.Minecraft.Data.Schema.JsonConverters;

namespace SteveClient.Minecraft.Data.Schema.BlockStates;

public class BlockStateJson
{
    [JsonPropertyName("variants"), JsonConverter(typeof(VariantsJsonConverter))]
    public VariantModels? Variants { get; set; }
}