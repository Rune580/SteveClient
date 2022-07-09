using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SteveClient.Engine.Networking;
using SteveClient.Engine.Rendering.Font;
using SteveClient.Engine.Rendering.Utils;

namespace SteveClient.Engine.Rendering.Ui.Elements;

public class Button : BaseUiElement
{
    private readonly Color4 _normalColor = new(105, 105, 105, 255);
    private readonly Color4 _selectedColor = new(156, 156, 156, 255);
    private readonly Color4 _pressedColor = new(89, 89, 89, 255);
    
    private States _currentState;
    private string _label;
    
    public Button(Box2 rect, string label) : base(rect)
    {
        _currentState = States.Normal;
        _label = label;
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

        Vector2 labelPos = Rect.Min + (Rect.Max - Rect.Min) / 2f;
        
        FontRenderer.DrawTextScreenSpace(_label, labelPos, (1.5f / 4f), true);
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

        if (!MouseInside(mousePos))
            _currentState = States.Normal;
        
        if (_currentState == States.Selected)
            _currentState = States.Pressed;
    }

    protected override void OnMouseUp(Vector2 mousePos, MouseButton button)
    {
        if (button != MouseButton.Left)
            return;

        if (!MouseInside(mousePos) && _currentState == States.Pressed)
        {
            _currentState = States.Normal;
            return;
        }

        if (!MouseInside(mousePos) && _currentState == States.Normal)
            return;
        
        Console.WriteLine("Button Pressed!");
        
        MinecraftNetworkingClient.Instance!.Connect("127.0.0.1", 25565);
        
        _currentState = States.Selected;
    }

    private enum States
    {
        Normal,
        Selected,
        Pressed,
        Disabled
    }
}