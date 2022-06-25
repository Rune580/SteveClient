using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.ImGui;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering.Definitions;

namespace SteveClient.Engine;

public class SteveClientWindow : GameWindow
{
    private SteveGameLoop _gameLoop;
    private readonly List<IMenu> _menus = new();

    public ImGuiController ImGuiController = null!;

    public SteveClientWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        Title += $": OpenGL Version: {GL.GetString(StringName.Version)}";

        ImGuiController = new ImGuiController(ClientSize.X, ClientSize.Y);
        
        var logic = new CompositionRoot(ClientSize);
        _gameLoop = new SteveGameLoop(logic.Scheduler, logic.GraphicsScheduler);
        _gameLoop.SetGraphicsFrameRate(144);
        
        ShaderDefinitions.LoadShaders();
        VertexDefinitions.Init();

        GL.ClearColor(0, 0, 0.1f, 1);
        
        RegisterMenus();
    }

    private void RegisterMenus()
    {
        _menus.Add(new ServerTesting());
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

        ImGuiController.Update(this, (float)e.Time);
        
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

        foreach (var menu in _menus)
            menu.Draw();
        
        ImGuiController.Render();

        _gameLoop.TickGraphics(e.Time);

        foreach (var renderLayer in RenderLayerDefinitions.Instances)
        {
            renderLayer.Bind();
            renderLayer.RebuildBuffers();
            renderLayer.BeforeRender();
            renderLayer.Render();
            renderLayer.AfterRender();
        }
        
        SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
        base.OnUpdateFrame(e);
        
        CursorState = InputManager.CursorState;
        InputManager.UpdateState(KeyboardState, MouseState);
        Rendering.WindowState.IsFocused = IsFocused;

        _gameLoop.Tick(e.Time);
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        base.OnTextInput(e);

        ImGuiController.PressChar((char)e.Unicode);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
        
        ImGuiController.MouseScroll(e.Offset);
    }
}