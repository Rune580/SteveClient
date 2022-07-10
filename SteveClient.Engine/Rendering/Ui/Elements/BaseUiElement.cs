using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;

namespace SteveClient.Engine.Rendering.Ui.Elements;

public abstract class BaseUiElement
{
    public Box2 Rect;

    protected BaseUiElement(Box2 rect)
    {
        Rect = rect;
    }
    
    public void Draw()
    {
        Render();
    }
    
    protected virtual void Render() { }

    public void UpdateControls(MouseState mouseState, KeyboardState keyboardState)
    {
        MouseFunctions(mouseState);
        
        OnUpdateKeyboard(keyboardState);
    }

    public void Update(double elapsedTime)
    {
        OnUpdate(elapsedTime);
    }
    
    protected virtual void OnUpdate(double elapsedTime) { }

    private void MouseFunctions(MouseState mouseState)
    {
        if (InputManager.CursorState != CursorState.Normal)
            return;
        
        Vector2 mousePos = mouseState.Position;
        Vector2 lastMousePos = mouseState.PreviousPosition;

        bool currentInside = MouseInside(mousePos);
        bool lastInside = MouseInside(lastMousePos);
        
        if (currentInside && !lastInside)
        {
            OnEnter(mousePos);
        }
        else if (lastInside && !currentInside)
        {
            OnExit(mousePos);
        }

        for (int i = 0; i < 8; i++)
        {
            MouseButton button = (MouseButton)i;

            if (mouseState.IsButtonDown(button))
                OnMouseDown(mousePos, button);

            if (mouseState.IsButtonReleased(button))
                OnMouseUp(mousePos, button);
        }
    }

    protected virtual void OnUpdateKeyboard(KeyboardState state) { }

    protected virtual void OnEnter(Vector2 mousePos) { }
    
    protected virtual void OnExit(Vector2 mousePos) { }
    
    protected virtual void OnMouseDown(Vector2 mousePos, MouseButton button) { }
    
    protected virtual void OnMouseUp(Vector2 mousePos, MouseButton button) { }
    
    protected bool MouseInside(Vector2 mousePos)
    {
        return Rect.Min.X < mousePos.X
               && Rect.Min.Y < mousePos.Y
               && Rect.Max.X > mousePos.X
               && Rect.Max.Y > mousePos.Y;
    }

    public void CharTyped(char charCode)
    {
        OnChar(charCode);
    }
    
    protected virtual void OnChar(char charCode) { }

    private Vector2 MouseToScreenPos(Vector2 mousePos)
    {
        Vector2 screenSize = new Vector2(WindowState.ScreenSize.X / 2f, WindowState.ScreenSize.Y / 2f);
        Vector2 screenPos = new Vector2(mousePos.X - screenSize.X, screenSize.Y - mousePos.Y);

        return screenPos;
    }
}