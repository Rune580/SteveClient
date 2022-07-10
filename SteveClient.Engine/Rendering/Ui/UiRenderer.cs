using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.Rendering.Ui.Elements;

namespace SteveClient.Engine.Rendering.Ui;

public static class UiRenderer
{
    public static readonly List<BaseElementContainer> UiElements = new();

    public static void UpdateControls(MouseState mouseState, KeyboardState keyboardState)
    {
        foreach (var uiElement in UiElements)
            uiElement.UpdateControls(mouseState, keyboardState);
    }

    public static void Update(double elapsedTime)
    {
        foreach (var uiElement in UiElements)
            uiElement.Update(elapsedTime);
    }

    public static void CharTyped(char charCode)
    {
        foreach (var uiElement in UiElements)
            uiElement.CharTyped(charCode);
    }

    public static void Render()
    {
        foreach (var uiElement in UiElements)
            uiElement.Render();
    }
}