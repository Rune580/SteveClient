using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class ApplyVelocityToSimpleRigidBodiesEngine : IQueryingEntitiesEngine, IScheduledEngine
{
    public EntitiesDB entitiesDB { get; set; }
    
    public void Ready() { }
    
    public void Execute(float delta)
    {
        foreach (var ((rigidbodies, transforms, count), _) in entitiesDB.QueryEntities<SimpleRigidBodyComponent, TransformComponent>(GameGroups.SimpleRigidBodies.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var rigidbody = ref rigidbodies[i];
                ref var transform = ref transforms[i];

                var targetPosition = transform.Position + rigidbody.Velocity * delta;

                transform.Position = targetPosition;
                rigidbody.Velocity = Vector3.Zero;
            }
        }
    }
}