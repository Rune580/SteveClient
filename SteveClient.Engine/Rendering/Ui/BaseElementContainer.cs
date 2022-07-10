using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.Rendering.Ui.Elements;

namespace SteveClient.Engine.Rendering.Ui;

public abstract class BaseElementContainer
{
    protected readonly List<BaseUiElement> UiElements = new();
    
    protected BaseElementContainer()
    {
        RegisterElements(UiElements);
    }

    protected abstract void RegisterElements(in List<BaseUiElement> uiElements);

    public void UpdateControls(MouseState mouseState, KeyboardState keyboardState)
    {
        foreach (var uiElement in UiElements)
            uiElement.UpdateControls(mouseState, keyboardState);
    }
    
    public void Update(double elapsedTime)
    {
        foreach (var uiElement in UiElements)
            uiElement.Update(elapsedTime);
    }

    public void CharTyped(char charCode)
    {
        foreach (var uiElement in UiElements)
            uiElement.CharTyped(charCode);
    }

    public void Render()
    {
        OnRender();
        
        foreach (var uiElement in UiElements)
            uiElement.Draw();
    }
    
    protected virtual void OnRender() { }
}