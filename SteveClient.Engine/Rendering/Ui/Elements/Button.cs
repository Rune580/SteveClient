using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.Rendering.Utils;

namespace SteveClient.Engine.Rendering.Ui.Elements;

public class Button : BaseUiElement
{
    private readonly Color4 _normalColor = new(105, 105, 105, 255);
    private readonly Color4 _selectedColor = new(156, 156, 156, 255);
    private readonly Color4 _pressedColor = new(89, 89, 89, 255);

    private States _currentState;
    
    public Button(Box2 rect) : base(rect)
    {
        _currentState = States.Normal;
    }

    protected override void Render()
    {
        Color4 color = _currentState switch
        {
            States.Normal => _normalColor,
            States.Selected => _selectedColor,
            States.Pressed => _pressedColor,
            _ => throw new ArgumentOutOfRangeException()
        };
        
        UiRenderHelper.Quad(Rect, color);
    }

    protected override void OnEnter(Vector2 mousePos)
    {
        if (_currentState == States.Normal)
            _currentState = States.Selected;
    }

    protected override void OnExit(Vector2 mousePos)
    {
        if (_currentState == States.Selected)
            _currentState = States.Normal;
    }

    protected override void OnMouseDown(Vector2 mousePos, MouseButton button)
    {
        if (button != MouseButton.Left)
            return;
        
        if (_currentState == States.Selected)
            _currentState = States.Pressed;
    }

    protected override void OnMouseUp(Vector2 mousePos, MouseButton button)
    {
        if (button != MouseButton.Left)
            return;

        if (!MouseInside(mousePos) || _currentState != States.Pressed)
            return;
        
        Console.WriteLine("Button Pressed!");
        _currentState = States.Normal;
    }

    private enum States
    {
        Normal,
        Selected,
        Pressed,
        Disabled
    }
}