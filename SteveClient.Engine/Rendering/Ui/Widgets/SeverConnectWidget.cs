using OpenTK.Mathematics;
using SteveClient.Engine.Networking;
using SteveClient.Engine.Rendering.Ui.Elements;

namespace SteveClient.Engine.Rendering.Ui.Widgets;

public class SeverConnectWidget : BaseElementContainer
{
    private Button _button;
    
    protected override void RegisterElements(in List<BaseUiElement> uiElements)
    {
        _button = new Button(new Box2(30, 100, 30 + 100, 100 + 24), "Connect");
        _button.ButtonPressed += ConnectToServer;

        uiElements.Add(_button);
    }

    private void ConnectToServer()
    {
        MinecraftNetworkingClient.Instance!.Connect("127.0.0.1", 25565);
    }
}