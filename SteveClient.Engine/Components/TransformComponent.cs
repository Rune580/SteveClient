using OpenTK.Mathematics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct TransformComponent : IEntityComponent
{
    public Vector3 Position;
    public Quaternion Rotation;

    public TransformComponent(in Vector3 position, in Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public TransformComponent(in Vector3 position) : this(position, Quaternion.FromEulerAngles(Vector3.Zero)) { }
    
    public TransformComponent() : this(new Vector3()) { }

    public new string ToString() => $"pos: {Position.ToString()}, rot: {Rotation.ToEulerAngles().ToString()}";
}

public static class TransformExtensions
{
    public static void Rotate(this ref TransformComponent transform, float yaw, float pitch)
    {
        Vector3 angles = transform.Rotation.ToEulerAngles();
        transform.Rotation = Quaternion.FromEulerAngles(pitch, yaw, angles.Z);
    }
}