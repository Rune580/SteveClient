using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct CameraControllerComponent : IEntityComponent
{
    public float Speed;

    public CameraControllerComponent(float speed)
    {
        Speed = speed;
    }
}