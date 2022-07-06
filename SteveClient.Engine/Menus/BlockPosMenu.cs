using ImGuiNET;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Menus;

public class BlockPosMenu : IMenu
{
    public static Vector3 Position
    {
        get => new(_direction.X, _direction.Y, _direction.Z);
        set => _direction = new System.Numerics.Vector3(value.X, value.Y, value.Z);
    }

    private static System.Numerics.Vector3 _direction;
    
    public void Draw()
    {
        ImGui.Begin("Position");

        ImGui.SliderFloat3("Dir", ref _direction, -20, 20);
        
        ImGui.End();
    }
}