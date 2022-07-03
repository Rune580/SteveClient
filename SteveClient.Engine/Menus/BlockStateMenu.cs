using System.Text;
using ImGuiNET;
using SteveClient.Engine.Engines.Tools;

namespace SteveClient.Engine.Menus;

public class BlockStateMenu : IMenu
{
    private readonly byte[] _resourceNameBuffer = new byte[100];
    public void Draw()
    {
        ImGui.Begin("BlockState");

        ImGui.InputText("ResourceName", _resourceNameBuffer, 100, ImGuiInputTextFlags.CharsNoBlank);

        var shouldLoad = ImGui.Button("Load");

        if (shouldLoad)
        {
            string resourceName = Encoding.UTF8.GetString(_resourceNameBuffer).Replace("\0", "");
            SpawnBlockModelEntityEngine.LoadBlockState(resourceName);
        }

        ImGui.End();
    }
}