using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteveClient.Engine.InputManagement;

public readonly struct KbmButton
{
    private readonly KbmType _type;
    private readonly int _index;

    public KbmButton(int index, KbmType type)
    {
        _index = index;
        _type = type;
    }

    public KbmButton(Keys key) : this((int)key, KbmType.Keyboard) { }

    public KbmButton(MouseButton button) : this((int)button, KbmType.Mouse) { }

    public bool IsPressed()
    {
        return _type switch
        {
            KbmType.Keyboard => InputManager.KeyboardState.IsKeyPressed(AsKey),
            KbmType.Mouse => InputManager.MouseState.IsButtonPressed(AsMouse),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool IsDown()
    {
        return _type switch
        {
            KbmType.Keyboard => InputManager.KeyboardState.IsKeyDown(AsKey),
            KbmType.Mouse => InputManager.MouseState.IsButtonDown(AsMouse),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool IsReleased()
    {
        return _type switch
        {
            KbmType.Keyboard => InputManager.KeyboardState.IsKeyReleased(AsKey),
            KbmType.Mouse => InputManager.MouseState.IsButtonReleased(AsMouse),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public new string ToString()
    {
        return _type switch
        {
            KbmType.Keyboard => AsKey.ToString(),
            KbmType.Mouse => AsMouse.ToString(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    private Keys AsKey => (Keys)_index;
    private MouseButton AsMouse => (MouseButton)_index;
    
    public bool IsKeyboard => _type == KbmType.Keyboard;
    public bool IsMouse => _type == KbmType.Mouse;

    public static implicit operator KbmButton(Keys key) => new(key);
    public static implicit operator KbmButton(MouseButton button) => new(button);

    public enum KbmType
    {
        Keyboard,
        Mouse
    }
}