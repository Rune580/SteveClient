using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.ImGui;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Ui;
using SteveClient.Engine.Rendering.Ui.Elements;
using SteveClient.Engine.Rendering.Ui.Widgets;

namespace SteveClient.Engine;

public class SteveClientWindow : GameWindow
{
    private SteveGameLoop _gameLoop;

    public ImGuiController ImGuiController = null!;

    public SteveClientWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

        ImGuiController = new ImGuiController(ClientSize.X, ClientSize.Y);
        
        var logic = new CompositionRoot();
        _gameLoop = new SteveGameLoop(logic.Scheduler, logic.GraphicsScheduler);
        _gameLoop.SetGraphicsFrameRate(144);
        
        ShaderDefinitions.LoadShaders();
        VertexDefinitions.Init();

        GL.ClearColor(0, 0, 0.1f, 1);
        
        UiRenderer.UiElements.Add(new BlockStateLoaderWidget());
        UiRenderer.UiElements.Add(new SeverConnectWidget());
    }
    
    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        
        ImGuiController.WindowResized(ClientSize.X, ClientSize.Y);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        
        _gameLoop.TickGraphics(e.Time);
        
        UiRenderer.Update(e.Time);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        foreach (var renderLayer in RenderLayerDefinitions.Instances)
        {
            renderLayer.Bind();
            renderLayer.PreRender();
            renderLayer.Render();
            renderLayer.PostRender();
        }

        ImGuiController.Update(this, (float)e.Time);

        ImGuiController.Render();

        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        
        CursorState = InputManager.CursorState;
        Cursor = InputManager.Cursor;
        
        InputManager.UpdateState(KeyboardState, MouseState, MousePosition);
        Rendering.WindowState.Update(IsFocused, ClientSize);

        UiRenderer.UpdateControls(MouseState, KeyboardState);

        _gameLoop.Tick(e.Time);
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);
        
        UiRenderer.CharTyped((char)e.Unicode);
        
        ImGuiController.PressChar((char)e.Unicode);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        
        ImGuiController.MouseScroll(e.Offset);
    }
}