using SteveClient.Engine.Networking.Protocol;
using SteveClient.Minecraft.Settings;

namespace SteveClient.Engine.Networking.Packets.ServerBound.Play;

public class ClientSettingsPacket : ServerBoundPacket
{
    public String Locale = "en_US";
    public sbyte ViewDistance = ClientSettings.RenderDistance;
    public ChatMode ChatMode = ChatMode.Enabled;
    public bool ChatColors = true;
    public DisplayedSkinParts DisplayedSkinParts = DisplayedSkinParts.All;
    public MainHand MainHand = MainHand.Right;
    public bool EnableTextFiltering = false;
    public bool AllowServerListings = true;

    public override void Write(in OutPacketBuffer packetBuffer)
    {
        packetBuffer.WriteString(Locale);
        packetBuffer.WriteSignedByte(ViewDistance);
        packetBuffer.WriteEnum(ChatMode);
        packetBuffer.WriteBool(ChatColors);
        packetBuffer.WriteEnum(DisplayedSkinParts);
        packetBuffer.WriteEnum(MainHand);
        packetBuffer.WriteBool(EnableTextFiltering);
        packetBuffer.WriteBool(AllowServerListings);
    }
}