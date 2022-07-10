using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Rendering.Font;
using SteveClient.Engine.Rendering.Utils;

namespace SteveClient.Engine.Rendering.Ui.Elements;

public class InputField : BaseUiElement
{
    private bool _hasFocus;
    private string _text;

    private double _backspaceTimer;
    private bool _backspaceHeld;
    
    public InputField(Box2 rect) : base(rect)
    {
        _text = "";
    }

    public string Text => _text;

    protected override void Render()
    {
        UiRenderHelper.Quad(Rect, Color4.Gray);
        
        Vector2 size = (Rect.Max - Rect.Min);
        Vector2 labelPos = Rect.Min + new Vector2(5, 0);
        
        FontRenderer.DrawTextScreenSpace(_text, labelPos, (1.5f / 4f));
    }

    protected override void OnUpdate(double elapsedTime)
    {
        if (_backspaceHeld)
            _backspaceTimer += elapsedTime;
    }

    protected override void OnUpdateKeyboard(KeyboardState state)
    {
        HandleTextOperations(state);
    }

    private void HandleTextOperations(KeyboardState state)
    {
        if (!_hasFocus)
            return;
        
        HandleBackspace(state);
    }

    private void HandleBackspace(KeyboardState state)
    {
        bool keyDown = state.IsKeyDown(Keys.Backspace);
        bool keyWasDown = state.WasKeyDown(Keys.Backspace);

        if (keyDown && !keyWasDown)
        {
            _backspaceHeld = true;
            RemoveCharFromEnd();
        }
        else if (!keyDown && keyWasDown)
        {
            _backspaceHeld = false;
            _backspaceTimer = 0;
        }

        if (_backspaceTimer >= 1)
        {
            _backspaceTimer -= 0.05;
            RemoveCharFromEnd();
        }
    }

    private void RemoveCharFromEnd()
    {
        if (string.IsNullOrEmpty(_text))
            return;
        
        _text = _text[..^1];
    }

    protected override void OnEnter(Vector2 mousePos)
    {
        InputManager.Cursor = MouseCursor.IBeam;
    }

    protected override void OnExit(Vector2 mousePos)
    {
        InputManager.Cursor = MouseCursor.Default;
    }

    protected override void OnMouseDown(Vector2 mousePos, MouseButton button)
    {
        if (button != MouseButton.Left)
            return;

        _hasFocus = MouseInside(mousePos);
    }

    protected override void OnChar(char charCode)
    {
        if (!_hasFocus)
            return;
        
        _text += charCode;
    }
}