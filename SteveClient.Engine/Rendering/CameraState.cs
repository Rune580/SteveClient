using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering;

public static class CameraState
{
    public static Matrix4 ViewMatrix { get; private set; }
    public static Matrix4 ProjectionMatrix { get; private set; }
    
    public static void Update(Matrix4 viewMatrix, Matrix4 projectionMatrix)
    {
        ViewMatrix = viewMatrix;
        ProjectionMatrix = projectionMatrix;
    }
}