using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteveClient.Engine.InputManagement;

public static class InputManager
{
    public static KeyboardState KeyboardState { get; private set; }
    public static MouseState MouseState { get; private set; }
    public static Vector2 MousePosition { get; private set; }
    public static CursorState CursorState = CursorState.Normal;
    
    internal static void UpdateState(KeyboardState keyboardState, MouseState mouseState, Vector2 mousePosition)
    {
        KeyboardState = keyboardState;
        MouseState = mouseState;
        MousePosition = mousePosition;
    }
}