using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.InputManagement;
using SteveClient.Engine.Menus;
using Svelto.ECS;
using WindowState = SteveClient.Engine.Rendering.WindowState;

namespace SteveClient.Engine.Engines;

public class CameraControlsEngine : BaseEngine
{
    public const float MouseSensitivity = 0.2f;
    public const float CameraSpeed = 1000f;
    public const float SprintModifier = 2f;
    public const float SneakModifier = 0.5f;
    
    private bool _firstMove = true;
    private Vector2 _lastPos;
    
    public override void Execute(float delta)
    {
        if (!WindowState.IsFocused)
            return;
        
        var rigidBodyCameraOptional = 
            entitiesDB.QueryUniqueEntityOptional<SimpleRigidBodyComponent, CameraComponent>(GameGroups.MainCamera.BuildGroup);

        if (rigidBodyCameraOptional.HasValue)
            HandleCameraMovementAndRotation(ref rigidBodyCameraOptional.Get1(), ref rigidBodyCameraOptional.Get2(), delta);

        var cameraTransformOptional =
            entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.MainCamera.BuildGroup);
        var playerTransformOptional =
            entitiesDB.QueryUniqueEntityOptional<TransformComponent>(GameGroups.Player.BuildGroup);

        if (cameraTransformOptional.HasValue && playerTransformOptional.HasValue)
            TeleportCameraToPlayer(ref cameraTransformOptional.Get1(), ref playerTransformOptional.Get1());
    }

    private void HandleCameraMovementAndRotation(ref SimpleRigidBodyComponent rigidBody, ref CameraComponent camera, float delta)
    {
        if (KeyBinds.ControlCamera.IsReleased && InputManager.CursorState == CursorState.Grabbed)
        {
            InputManager.CursorState = CursorState.Normal;
            _firstMove = true;
        }
        
        if (!KeyBinds.ControlCamera.IsDown)
            return;

        InputManager.CursorState = CursorState.Grabbed;

        float modifier = GetSpeedModifier();

        Vector3 velocity = Vector3.Zero;
        
        if (KeyBinds.CameraForward.IsDown)
            velocity += camera.Front * (CameraSpeed * modifier * delta);
        if (KeyBinds.CameraBackwards.IsDown)
            velocity -= camera.Front * (CameraSpeed * modifier * delta);
        if (KeyBinds.CameraLeft.IsDown)
            velocity -= camera.Right * (CameraSpeed * modifier * delta);
        if (KeyBinds.CameraRight.IsDown)
            velocity += camera.Right * (CameraSpeed * modifier * delta);
        if (KeyBinds.CameraUp.IsDown)
            velocity += camera.Up * (CameraSpeed * modifier * delta);
        if (KeyBinds.CameraDown.IsDown)
            velocity -= camera.Up * (CameraSpeed * modifier * delta);

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

    private float GetSpeedModifier()
    {
        return KeyBinds.Sneak.IsDown ? SneakModifier : KeyBinds.Sprint.IsDown ? SprintModifier : 1f;
    }

    private void TeleportCameraToPlayer(ref TransformComponent cameraTransform, ref TransformComponent playerTransform)
    {
        DebugWidget.CameraPos = cameraTransform.Position;
        DebugWidget.PlayerPos = playerTransform.Position;
        
        if (!KeyBinds.TeleportToPlayer.IsPressed)
            return;

        cameraTransform.Position = playerTransform.Position;
    }
}