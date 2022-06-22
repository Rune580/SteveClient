using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteveClient.Engine.InputManagement;

public static class InputManager
{
    public static KeyboardState KeyboardState = null!;
    public static MouseState MouseState = null!;
    public static CursorState CursorState = CursorState.Normal;
    
    internal static void UpdateState(KeyboardState keyboardState, MouseState mouseState)
    {
        KeyboardState = keyboardState;
        MouseState = mouseState;
    }
}