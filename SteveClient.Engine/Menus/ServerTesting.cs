using System.Text;
using ImGuiNET;
using SteveClient.Engine.Networking;

namespace SteveClient.Engine.Menus;

public class ServerTesting : IMenu
{
    private readonly byte[] _addressBuffer = new byte[100];
    private int _port = 25565;
    
    public void Draw()
    {
        ImGui.Begin("Server Testing");
        
        ImGui.InputText("Address", _addressBuffer, 100);
        ImGui.InputInt("Port", ref _port);

        if (ImGui.Button("Connect"))
            MinecraftNetworkingClient.Instance!.Connect(Encoding.UTF8.GetString(_addressBuffer).Trim(), (ushort)_port);
        
        ImGui.End();
    }
}