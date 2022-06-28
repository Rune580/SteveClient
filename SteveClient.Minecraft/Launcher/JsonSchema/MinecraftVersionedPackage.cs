using System.Text.Json.Serialization;

namespace SteveClient.Minecraft.Launcher.JsonSchema;

public class MinecraftVersionedPackage
{
    [JsonPropertyName("downloads")]
    public DownloadsEntry Downloads { get; set; }
    
    public class DownloadsEntry
    {
        [JsonPropertyName("client")]
        public SubDownloadEntry Client { get; set; }
        
        [JsonPropertyName("client_mappings")]
        public SubDownloadEntry ClientMappings { get; set; }
        
        [JsonPropertyName("server")]
        public SubDownloadEntry Server { get; set; }
        
        [JsonPropertyName("server_mappings")]
        public SubDownloadEntry ServerMappings { get; set; }
        
        public class SubDownloadEntry
        {
            [JsonPropertyName("sha1")]
            public string Sha1 { get; set; }
            
            [JsonPropertyName("size")]
            public int Size { get; set; }
            
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }
    }
}