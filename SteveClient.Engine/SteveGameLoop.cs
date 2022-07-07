using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Font;

namespace SteveClient.Engine;

public class SteveGameLoop
{
    private const uint TicksPerSecond = 1000 * 1000;
    
    private readonly ScheduledAction _main;
    private readonly ScheduledAction _graphics;

    private uint _graphicsFrameRate;
    private double _lastDelta;
    private double _lastGraphicsDelta;

    public SteveGameLoop(EngineScheduler scheduler, EngineScheduler graphicsScheduler)
    {
        _lastDelta = 0UL;
        _graphicsFrameRate = 1;

        _main = new ScheduledAction(() => scheduler.Execute(GetLastDelta()), 1, false);
        
        _graphics = new ScheduledAction(() =>
        {
            RenderLayerDefinitions.FlushAll();
            
            FontRenderer.DrawText(new FontString("Ligma Balls"), new Vector3(0, 1, 4), (1f / 32f) / 4f, DirectionMenu.Direction);
            FontRenderer.DrawText(new FontString("Bigma Lalls"), new Vector3(0, 1, 5), (1f / 32f) / 4f, DirectionMenu.Direction, Color4.Red);

            var pos = InputManager.MousePosition;
            Vector2 screenSize = new Vector2(WindowState.ScreenSize.X / 2f, WindowState.ScreenSize.Y / 2f);
            Vector2 screenPos = new Vector2(pos.X - screenSize.X, screenSize.Y - pos.Y);
            
            FontRenderer.DrawTextScreenSpace($"x: {pos.X}, y: {pos.Y}", screenPos, 0.25f);

            graphicsScheduler.Execute(GetLastGraphicsDelta());
            RenderLayerDefinitions.RebuildAll();
        }, _graphicsFrameRate, true);
    }

    public void Tick(double elapsedTime)
    {
        ulong elapsedTicks = (ulong)(elapsedTime * TicksPerSecond);
        
        _main.Tick(elapsedTicks);

        _lastDelta = elapsedTime;
    }

    public void TickGraphics(double elapsedTime)
    {
        ulong elapsedTicks = (ulong)(elapsedTime * TicksPerSecond);
        
        _lastGraphicsDelta = elapsedTime;
        
        _graphics.Tick(elapsedTicks);
    }

    public SteveGameLoop SetGraphicsFrameRate(uint fps)
    {
        _graphicsFrameRate = TicksPerSecond / fps;
        _graphics.UpdateFrequency(_graphicsFrameRate);
        return this;
    }

    public float GetLastDelta()
    {
        return (float)_lastDelta;
    }

    public float GetLastGraphicsDelta()
    {
        return (float)_lastGraphicsDelta;
    }
}