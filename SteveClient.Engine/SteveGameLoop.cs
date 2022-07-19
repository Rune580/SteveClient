using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Font;
using SteveClient.Engine.Rendering.Textures.Atlas;
using SteveClient.Engine.Rendering.Ui;

namespace SteveClient.Engine;

public class SteveGameLoop
{
    private const uint TicksPerSecond = 1000 * 1000;
    
    private readonly ScheduledAction _main;
    private readonly ScheduledAction _graphics;
    private readonly ScheduledAction _gameTicks;

    private readonly EngineScheduler _graphicsScheduler; 

    private uint _graphicsFrameRate;
    private double _lastDelta;
    private double _lastGraphicsDelta;
    private float _graphicsTimer;
    private uint _framesRendered;
    private float _avgMsPerFrameASecond;

    public SteveGameLoop(EngineScheduler scheduler, EngineScheduler graphicsScheduler)
    {
        _graphicsScheduler = graphicsScheduler;
        
        _lastDelta = 0UL;
        _graphicsFrameRate = 1;
        _graphicsTimer = 0;
        _framesRendered = 1;
        _avgMsPerFrameASecond = 0;

        _main = new ScheduledAction(() => scheduler.Execute(GetLastDelta()), 1, false);
        _graphics = new ScheduledAction(Render, _graphicsFrameRate, false);

        _gameTicks = new ScheduledAction(() => AtlasAnimatedTexture.TickAnimations(), TicksPerSecond / 20, true);
    }
    
    private void Render()
    {
        float milliseconds = GetLastGraphicsDelta() * 1000;
        _graphicsTimer += milliseconds;

        if (_graphicsTimer >= 1)
        {
            _avgMsPerFrameASecond = _graphicsTimer / _framesRendered;
            _framesRendered = 0;
            _graphicsTimer -= _graphicsTimer;
        }
        
        RenderLayerDefinitions.FlushAll();

        UiRenderer.Render();

        FontRenderer.DrawTextScreenSpace($"{1000 / _avgMsPerFrameASecond:F0}Fps, {_avgMsPerFrameASecond:F0}ms per frame", new Vector2(0, 0), (1.5f / 4f));

        // FontRenderer.DrawText(new FontString("Ligma Balls"), new Vector3(0, 1, 4), (1f / 32f) / 4f, DirectionMenu.Direction);
        // FontRenderer.DrawText(new FontString("Bigma Lalls"), new Vector3(0, 1, 5), (1f / 32f) / 4f, DirectionMenu.Direction, Color4.Red);

        _graphicsScheduler.Execute(GetLastGraphicsDelta());
        RenderLayerDefinitions.RebuildAll();

        _framesRendered++;
    }

    public void Tick(double elapsedTime)
    {
        ulong elapsedTicks = (ulong)(elapsedTime * TicksPerSecond);
        
        _main.Tick(elapsedTicks);
        _gameTicks.Tick(elapsedTicks);

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