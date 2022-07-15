using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Data.Schema.Textures;

public class TextureMcMetaJson
{
    [JsonPropertyName("animation")]
    public AnimationJson Animation { get; set; }
}