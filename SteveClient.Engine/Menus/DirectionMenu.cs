using ImGuiNET;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Menus;

public class DirectionMenu : IMenu
{
    public static Vector3 Direction
    {
        get => new(_direction.X, _direction.Y, _direction.Z);
        set => _direction = new System.Numerics.Vector3(value.X, value.Y, value.Z);
    }

    private static System.Numerics.Vector3 _direction;
    
    public void Draw()
    {
        ImGui.Begin("Direction");

        ImGui.SliderFloat3("Dir", ref _direction, -1, 1);
        
        ImGui.End();
    }
}