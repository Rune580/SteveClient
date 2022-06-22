using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Rendering;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class DoMovementOnControllableCamerasEngine : IQueryingEntitiesEngine, IScheduledEngine
{
    public const float MouseSensitivity = 0.2f;
    
    private bool _firstMove = true;
    private Vector2 _lastPos;
    
    public EntitiesDB entitiesDB { get; set; }
    
    public void Ready() { }
    
    public void Execute(float delta)
    {
        foreach (var ((rigidBodies, cameras, cameraControllers, count), _) in entitiesDB.QueryEntities<SimpleRigidBodyComponent, CameraComponent, CameraControllerComponent>(GameGroups.ControllableCameras.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var rigidBody = ref rigidBodies[i];
                ref var camera = ref cameras[i];
                ref var cameraController = ref cameraControllers[i];

                HandleCameraMovement(ref rigidBody, ref camera, ref cameraController);
            }
        }
    }

    private void HandleCameraMovement(ref SimpleRigidBodyComponent rigidBody, ref CameraComponent camera, ref CameraControllerComponent cameraController)
    {
        if (!WindowState.IsFocused)
            return;
        
        Vector3 velocity = Vector3.Zero;
        
        if (KeyBinds.CameraForward.IsDown)
            velocity += camera.Front * cameraController.Speed;
        if (KeyBinds.CameraBackwards.IsDown)
            velocity -= camera.Front * cameraController.Speed;
        if (KeyBinds.CameraLeft.IsDown)
            velocity -= camera.Right * cameraController.Speed;
        if (KeyBinds.CameraRight.IsDown)
            velocity += camera.Right * cameraController.Speed;
        if (KeyBinds.CameraUp.IsDown)
            velocity += camera.Up * cameraController.Speed;
        if (KeyBinds.CameraDown.IsDown)
            velocity -= camera.Up * cameraController.Speed;

        rigidBody.Velocity = velocity;

        var mouse = InputManager.MouseState;

        if (_firstMove)
        {
            _lastPos = new Vector2(mouse.X, mouse.Y);
            _firstMove = false;
        }
        else
        {
            var deltaX = mouse.X - _lastPos.X;
            var deltaY = mouse.Y - _lastPos.Y;
            _lastPos = new Vector2(mouse.X, mouse.Y);

            camera.Yaw += deltaX * MouseSensitivity;
            camera.Pitch -= deltaY * MouseSensitivity;
        }
    }
}