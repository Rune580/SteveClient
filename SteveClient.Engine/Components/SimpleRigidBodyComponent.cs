using OpenTK.Mathematics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct SimpleRigidBodyComponent : IEntityComponent
{
    public Vector3 Velocity;

    public SimpleRigidBodyComponent(Vector3 velocity)
    {
        Velocity = velocity;
    }

    public SimpleRigidBodyComponent() : this(new Vector3()) { }

    public new string ToString() => $"vel: {Velocity.ToString()}";

    public void AddVelocity(Vector3 velocity)
    {
        Velocity += velocity;
    }
}