using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using SteveClient.Engine.Rendering;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class CameraControlsEngine : BaseEngine
{
    public const float MouseSensitivity = 0.2f;
    public const float CameraSpeed = 1.5f;
    
    private bool _firstMove = true;
    private Vector2 _lastPos;
    
    public override void Execute(float delta)
    {
        if (!WindowState.IsFocused)
            return;
        
        var rigidBodyCameraOptional = 
            entitiesDB.QueryUniqueEntityOptional<SimpleRigidBodyComponent, CameraComponent>(GameGroups.MainCamera.BuildGroup);

        if (rigidBodyCameraOptional.HasValue)
            HandleCameraMovement(ref rigidBodyCameraOptional.Get1(), ref rigidBodyCameraOptional.Get2());

        var cameraTransformOptional =
            entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.MainCamera.BuildGroup);
        var playerTransformOptional =
            entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.Player.BuildGroup);

        if (cameraTransformOptional.HasValue && playerTransformOptional.HasValue)
            TeleportCameraToPlayer(ref cameraTransformOptional.Get1(), ref playerTransformOptional.Get1());
    }

    private void HandleCameraMovement(ref SimpleRigidBodyComponent rigidBody, ref CameraComponent camera)
    {
        Vector3 velocity = Vector3.Zero;
        
        if (KeyBinds.CameraForward.IsDown)
            velocity += camera.Front * CameraSpeed;
        if (KeyBinds.CameraBackwards.IsDown)
            velocity -= camera.Front * CameraSpeed;
        if (KeyBinds.CameraLeft.IsDown)
            velocity -= camera.Right * CameraSpeed;
        if (KeyBinds.CameraRight.IsDown)
            velocity += camera.Right * CameraSpeed;
        if (KeyBinds.CameraUp.IsDown)
            velocity += camera.Up * CameraSpeed;
        if (KeyBinds.CameraDown.IsDown)
            velocity -= camera.Up * CameraSpeed;

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

    private void TeleportCameraToPlayer(ref TransformComponent cameraTransform, ref TransformComponent playerTransform)
    {
        if (!KeyBinds.TeleportToPlayer.IsPressed)
            return;

        cameraTransform.Position = playerTransform.Position;
    }
}