using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering;

public static class WindowState
{
    public static bool IsFocused { get; private set; }
    public static Vector2i ScreenSize { get; private set; }

    public static void Update(bool isFocused, Vector2i screenSize)
    {
        IsFocused = isFocused;
        ScreenSize = screenSize;
    }
}