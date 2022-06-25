namespace SteveClient.Minecraft.Settings;

[Flags]
public enum DisplayedSkinParts
{
    Cape = 1,
    Jacket = 2,
    LeftSleeve = 4,
    RightSleeve = 8,
    LeftPantsLeg = 16,
    RightPantsLeg = 32,
    Hat = 64,
    Unused = 128,
    All = 127
}