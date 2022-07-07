using OpenTK.Mathematics;
using SteveClient.Engine.Rendering;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct CameraComponent : IEntityComponent
{
    public bool IsActive;
    
    // Direction vectors
    public Vector3 Front;
    public Vector3 Up;
    public Vector3 Right;
    
    // Look
    private float _pitch;
    private float _yaw;
    
    private float _fov;

    public CameraComponent()
    {
        IsActive = true;
        
        Front = -Vector3.UnitZ;
        Up = Vector3.UnitY;
        Right = Vector3.UnitX;

        _pitch = 0;
        _yaw = -MathHelper.PiOver2;
        _fov = MathHelper.PiOver2;
    }

    public float Pitch
    {
        get => MathHelper.RadiansToDegrees(_pitch);
        set
        {
            var angle = MathHelper.Clamp(value, -89f, 89f);
            _pitch = MathHelper.DegreesToRadians(angle);
            UpdateVectors();
        }
    }

    public float Yaw
    {
        get => MathHelper.RadiansToDegrees(_yaw);
        set
        {
            _yaw = MathHelper.DegreesToRadians(value);
            UpdateVectors();
        }
    }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1f, 103f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    public float AspectRatio => WindowState.ScreenSize.X / (float)WindowState.ScreenSize.Y;

    public Matrix4 GetViewMatrix(Vector3 position) => Matrix4.LookAt(position, position + Front, Up);

    public Matrix4 GetProjectionMatrix() => Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100f);
    
    public Matrix4 GetScreenSpaceMatrix() => Matrix4.CreateOrthographic(WindowState.ScreenSize.X, WindowState.ScreenSize.Y, 0.01f, 1000f);

    private void UpdateVectors()
    {
        Front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
        Front.Y = MathF.Sin(_pitch);
        Front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

        Front = Vector3.Normalize(Front);

        Right = Vector3.Normalize(Vector3.Cross(Front, Vector3.UnitY));
        Up = Vector3.Normalize(Vector3.Cross(Right, Front));
    }
}