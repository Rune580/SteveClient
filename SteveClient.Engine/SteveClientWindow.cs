using OpenTK.ImGui;
using OpenTK.Windowing.Desktop;

namespace SteveClient.Engine;

public class SteveClientWindow : GameWindow
{
    public ImGuiController ImGuiController;
    
    public SteveClientWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        ImGuiController = new ImGuiController(ClientSize.X, ClientSize.Y);
    }
}