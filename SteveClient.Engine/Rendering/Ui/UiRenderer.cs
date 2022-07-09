using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.Rendering.Ui.Elements;

namespace SteveClient.Engine.Rendering.Ui;

public static class UiRenderer
{
    public static readonly List<BaseUiElement> UiElements = new();

    public static void UpdateControls(MouseState mouseState, KeyboardState keyboardState)
    {
        foreach (var uiElement in UiElements)
            uiElement.UpdateControls(mouseState, keyboardState);
    }

    public static void Render()
    {
        foreach (var uiElement in UiElements)
            uiElement.Draw();
    }
}