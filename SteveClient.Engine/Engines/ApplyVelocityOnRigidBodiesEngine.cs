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
        HandleGravity(ref rigidBody, transform.Position);
        
        if (rigidBody.Velocity == Vector3d.Zero)
            return;

        Vector3d nextPosition = transform.Position + rigidBody.Velocity;
        VoxelShape collisions = GetCollisionShapeForMovement(nextPosition, rigidBody.BoxCollider);

        Vector3d collisionMovement = AdjustMovementForCollisions(rigidBody.Velocity, rigidBody.BoxCollider.Offset(nextPosition), collisions);
        
        float friction = GetFriction(rigidBody.BoxCollider, transform.Position);
        ReduceVelocity(ref rigidBody, friction);

        transform.Position += (Vector3)collisionMovement;
    }

    private void HandleGravity(ref RigidBodyComponent rigidBody, Vector3 pos)
    {
        Aabb feet = new Aabb(new Vector3d(-0.3d, -0.001d, -0.3d), new Vector3d(0.3d, 0, 0.3d));
        Aabb aabb = feet.Offset(pos);
        var blocks = aabb.GetBlockPositions();

        rigidBody.OnGround = false;

        foreach (var blockPos in blocks)
        {
            if (_world.GetBlockState(blockPos).CollisionShape.Offset(blockPos).Intersects(aabb))
                rigidBody.OnGround = true;
        }

        if (rigidBody.OnGround)
            return;

        double velocity = rigidBody.Velocity.Y;

        velocity -= 0.08f;
        velocity *= 0.2f;

        rigidBody.Velocity.Y = velocity;
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

        if (velocity.X < 1.0E-7)
            velocity.X = 0;
        if (velocity.Y < 1.0E-7)
            velocity.Y = 0;
        if (velocity.Z < 1.0E-7)
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

    private Vector3d AdjustMovementForCollisions(Vector3d movement, Aabb aabb, VoxelShape collisions)
    {
        Aabb adjustedAabb = new Aabb(aabb);
        Vector3d adjustedMovement = new Vector3d(movement);

        adjustedMovement.Y = CheckMovement(collisions, ref adjustedAabb, Directions.Down, adjustedMovement.Y, true);

        bool zHasPriority = adjustedMovement.Z > adjustedMovement.X;

        if (zHasPriority)
            adjustedMovement.Z = CheckMovement(collisions, ref adjustedAabb, Directions.North, adjustedMovement.Z, true);

        adjustedMovement.X = CheckMovement(collisions, ref adjustedAabb, Directions.East, adjustedMovement.X, true);

        if (!zHasPriority)
            adjustedMovement.Z = CheckMovement(collisions, ref adjustedAabb, Directions.North, adjustedMovement.Z, false);

        if (adjustedMovement.Length > movement.Length)
            return Vector3d.Zero;

        return adjustedMovement;
    }

    private double CheckMovement(in VoxelShape collisions, ref Aabb adjustedAabb, Directions direction, double origValue, bool offsetAabb)
    {
        double value = origValue;
        if (value == 0 || Math.Abs(value) < 1.0E-7)
            return 0;

        value = collisions.ComputeOffset(adjustedAabb, value, direction);

        if (offsetAabb && value != 0)
        {
            Vector3d offset = direction switch
            {
                Directions.East or Directions.West=> new Vector3d(value, 0, 0),
                Directions.Up or Directions.Down => new Vector3d(0, value, 0),
                Directions.North or Directions.South => new Vector3d(0, 0, value),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };

            adjustedAabb = adjustedAabb.Offset(offset);
        }

        return value;
    }
}