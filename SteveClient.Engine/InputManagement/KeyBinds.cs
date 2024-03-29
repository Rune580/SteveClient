﻿using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteveClient.Engine.InputManagement;

public static class KeyBinds
{
    public static readonly KeyBind Sprint = new("Sprint", Keys.LeftShift);
    public static readonly KeyBind Sneak = new("Sneak", Keys.LeftControl);
    public static readonly KeyBind CameraForward = new("Camera Forward", Keys.W);
    public static readonly KeyBind CameraLeft = new("Camera Left", Keys.A);
    public static readonly KeyBind CameraBackwards = new("Camera Backwards", Keys.S);
    public static readonly KeyBind CameraRight = new("Camera Right", Keys.D);
    public static readonly KeyBind CameraUp = new("Camera Up", Keys.Q);
    public static readonly KeyBind CameraDown = new("Camera Down", Keys.E);
    public static readonly KeyBind ControlCamera = new("Control Camera", MouseButton.Right);
    public static readonly KeyBind Pause = new("Pause", Keys.Escape);
    public static readonly KeyBind TeleportToPlayer = new("Teleport to Player", Keys.T);
    public static readonly KeyBind ReloadChunks = new("Reload Chunks", Keys.F5);
    
    public struct KeyBind
    {
        public readonly string Name;
        public KbmButton Button;

        public KeyBind(string name, KbmButton button)
        {
            Name = name;
            Button = button;
        }

        public new string ToString() => $"name: {Name}, button: {Button}, state: ({IsPressed}, {IsDown}, {IsReleased})";

        public bool IsPressed => Button.IsPressed();
        public bool IsDown => Button.IsDown();
        public bool IsReleased => Button.IsReleased();
    }
}