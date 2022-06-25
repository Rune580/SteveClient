using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.DataGen.Generators;

namespace SteveClient.Minecraft.DataGen;

public static class DataGenerator
{
    public static void GenerateData()
    {
        BlocksGen.GenerateBlocks();
    }
}