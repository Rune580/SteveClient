using System.Text.Json;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Utils;

namespace SteveClient.Minecraft.DataGen.Generators;

public static class BlocksGen
{
    private const string ArticBaseUrl = "https://raw.githubusercontent.com/Articdive/ArticData/";

    public static void GenerateBlocks()
    {
        ParseBlocksJson();
    }

    public static void ParseBlocksJson()
    {
        string jsonPath = GetBlocksJsonPath();

        List<Block> blocks = new List<Block>();
        
        using JsonDocument blockListDocument = JsonDocument.Parse(File.ReadAllText(jsonPath));
        var enumerator = blockListDocument.RootElement.EnumerateObject();

        while (enumerator.MoveNext())
        {
            JsonElement json = enumerator.Current.Value;
            
            Dictionary<int, BlockState> blockStates = new Dictionary<int, BlockState>();
            
            JsonElement blockStatesElement = json.GetProperty("states");
            int blockStatesLength = blockStatesElement.GetArrayLength();
            
            int blockId = json.GetProperty("id").GetInt32();
            
            for (int i = 0; i < blockStatesLength; i++)
            {
                var currentState = blockStatesElement[i];
                        
                int blockStateId = currentState.GetProperty("stateId").GetInt32();
                
                BlockState blockState = new BlockState(
                    blockId,
                    blockStateId,
                    currentState.GetProperty("collisionShape").GetString()!,
                    currentState.GetProperty("opacity").GetInt32(),
                    currentState.GetProperty("occludes").GetBoolean()
                );

                blockStates[blockStateId] = blockState;
            }
            
            Block block = new Block(
                blockId,
                enumerator.Current.Name,
                json.GetProperty("explosionResistance").GetSingle(),
                json.GetProperty("friction").GetSingle(),
                json.GetProperty("speedFactor").GetSingle(),
                json.GetProperty("jumpFactor").GetSingle(),
                json.GetProperty("defaultStateId").GetInt32(),
                blockStates
            );
            
            blocks.Add(block);
        }
        
        Data.Blocks.LoadFromArray(blocks.ToArray());
        blocks.Clear();
    }

    private static string GetBlocksJsonPath()
    {
        if (!Directory.Exists("data"))
            Directory.CreateDirectory("data");
        
        string path = Path.Join(Path.GetFullPath("data"), $"{MinecraftDefinition.Version.Replace('.', '_')}_blocks.json");

        if (!File.Exists(path))
        {
            string blocksUrl = $"{ArticBaseUrl}{MinecraftDefinition.Version}/{MinecraftDefinition.Version.Replace('.', '_')}_blocks.json";
            WebHelper.DownloadFile(blocksUrl, path);
        }

        return path;
    }
}