using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.ImGui;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using Serilog;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Ui;
using SteveClient.Engine.Rendering.Ui.Widgets;

namespace SteveClient.Engine;

public class SteveClientWindow : GameWindow
{
    private SteveGameLoop _gameLoop;

    public ImGuiController ImGuiController = null!;

    public SteveClientWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        
        Log.Debug("Instantiating main window");
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        
        //GL.Enable(EnableCap.DebugOutput);
        //GL.DebugMessageCallback(GlErrorCallback, IntPtr.Zero);

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

    private void GlErrorCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr messagePtr, IntPtr userParam)
    {
        // string? message = Marshal.PtrToStringUTF8(messagePtr, length);
        //
        // if (string.IsNullOrEmpty(message))
        //     return;
        
        //Log.Debug("Type: {Type}, Severity: {Severity}, Message: {Message}", type.ToString(), severity.ToString(), "message");
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

        RenderLayerDefinitions.Render();

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