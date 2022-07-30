using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Game;
using SteveClient.Minecraft.Numerics;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class ApplyVelocityOnRigidBodiesEngine : BaseEngine
{
    private readonly World _world;
    
    public ApplyVelocityOnRigidBodiesEngine(World world)
    {
        _world = world;
    }

    public override void Execute(float delta)
    {
        foreach (var ((transforms, rigidBodies, count), _) in entitiesDB
                     .QueryEntities<TransformComponent, RigidBodyComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var rigidBody = ref rigidBodies[i];

                if (rigidBody.Velocity == Vector3.Zero)
                    continue;
                
                HandlePhysics(ref rigidBody, ref transform);
            }
        }
    }

    private void HandlePhysics(ref RigidBodyComponent rigidBody, ref TransformComponent transform)
    {
        rigidBody.Velocity.Y = 0; //TODO: Gravity
        
        if (rigidBody.Velocity == Vector3d.Zero)
            return;
        
        float friction = GetFriction(rigidBody.BoxCollider, transform.Position);
        
        Vector3d nextPosition = transform.Position + rigidBody.Velocity;
        
        ReduceVelocity(ref rigidBody, friction);

        transform.Position = (Vector3)nextPosition;
    }

    private float GetFriction(Aabb aabb, Vector3 pos)
    {
        var blocks = aabb.Offset(pos).Offset(Directions.Down, 0.01f).GetBlockPositions();

        Vector3i blockPos = GetBlockClosestToPos(pos, blocks);

        return _world.GetBlockState(blockPos).Block.Friction;
    }

    private Vector3i GetBlockClosestToPos(Vector3 pos, List<Vector3i> blocks)
    {
        Vector3i closest = Vector3i.Zero;
        float distance = 100;
        
        foreach (var blockPos in blocks)
        {
            var dist = Vector3.Distance(pos, blockPos);

            if (!(dist < distance))
                continue;
            
            distance = dist;
            closest = blockPos;
        }

        return closest;
    }

    private void ReduceVelocity(ref RigidBodyComponent rigidBody, float friction)
    {
        Vector3d velocity = rigidBody.Velocity;
        
        velocity.X *= friction;
        velocity.Z *= friction;

        if (velocity.X < 1.0E-4)
            velocity.X = 0;
        if (velocity.Y < 1.0E-4)
            velocity.Y = 0;
        if (velocity.Z < 1.0E-4)
            velocity.Z = 0;

        rigidBody.Velocity = velocity;
    }

    private VoxelShape GetCollisionShapeForMovement(Vector3d pos, Aabb aabb)
    {
        var blockPositions = aabb.Offset(pos).Grow(0.001).Extend(Directions.Down).GetBlockPositions();

        VoxelShape result = new VoxelShape();

        foreach (var blockPosition in blockPositions)
        {
            VoxelShape collisionShape = _world.GetBlockState(blockPosition).CollisionShape.Offset(blockPosition);
            
            result.Add(collisionShape);
        }

        return result;
    }
}