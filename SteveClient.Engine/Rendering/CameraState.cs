using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering;

public static class CameraState
{
    public static Matrix4 ViewMatrix { get; private set; }
    public static Matrix4 ProjectionMatrix { get; private set; }
    public static Matrix4 ScreenSpaceViewMatrix { get; } = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
    public static Matrix4 ScreenSpaceProjectionMatrix { get; private set; }
    public static Vector3 Position { get; private set; }
    public static Vector3 Forward { get; private set; }
    public static Vector3 Up { get; private set; }
    public static Vector3 Right { get; private set; }
    
    public static void Update(Matrix4 viewMatrix, Matrix4 projectionMatrix, Matrix4 screenSpaceMatrix, Vector3 position, Vector3 forward, Vector3 up, Vector3 right)
    {
        ViewMatrix = viewMatrix;
        ProjectionMatrix = projectionMatrix;
        ScreenSpaceProjectionMatrix = screenSpaceMatrix;
        Position = position;
        Forward = forward;
        Up = up;
        Right = right;
    }
}