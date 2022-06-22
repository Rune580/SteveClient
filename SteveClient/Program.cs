using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SteveClient.Engine;

namespace SteveClient;

internal static class Program
{
    private static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings
        {
            Size = new Vector2i(1280, 720),
            Title = "Pain",
            Flags = ContextFlags.ForwardCompatible
        };

        using var window = new SteveClientWindow(GameWindowSettings.Default, nativeWindowSettings);
        
        window.Run();
    }
}