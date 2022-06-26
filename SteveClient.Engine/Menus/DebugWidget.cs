using ImGuiNET;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Menus;

public class DebugWidget : IMenu
{
    public static Vector3 CameraPos { set => _cameraPos = new System.Numerics.Vector3(value.X, value.Y, value.Z); }
    private static System.Numerics.Vector3 _cameraPos;
    
    public static Vector3 PlayerPos { set => _playerPos = new System.Numerics.Vector3(value.X, value.Y, value.Z); }
    private static System.Numerics.Vector3 _playerPos;
    
    public void Draw()
    {
        ImGui.Begin("Positions");

        ImGui.InputFloat3("Camera Pos", ref _cameraPos);

        ImGui.InputFloat3("Player Pos", ref _playerPos);
        
        ImGui.End();
    }
}