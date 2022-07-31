using OpenTK.Mathematics;
using SteveClient.Minecraft.Numerics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct RigidBodyComponent : IEntityComponent
{
    public Aabb BoxCollider;
    public Vector3d Velocity;
    public bool OnGround;

    public RigidBodyComponent(Aabb boxCollider, Vector3 initialVelocity, bool onGround)
    {
        BoxCollider = boxCollider;
        Velocity = initialVelocity;
        OnGround = onGround;
    }
    
    public RigidBodyComponent() : this(
        new Aabb(new Vector3(-0.3f, 0f, -0.3f), new Vector3(0.3f, 1.8f, 0.3f)),
        Vector3.Zero,
        true) { }

    public new string ToString() => $"Vel: {Velocity}";

}