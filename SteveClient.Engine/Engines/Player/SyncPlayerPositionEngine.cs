using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.ECS;
using SteveClient.Engine.Networking.Packets.ServerBound.Play;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Player;

public class SyncPlayerPositionEngine : BaseEngine
{
    private Vector3d _lastPosition = Vector3d.Zero;
    
    public override void Execute(float delta)
    {
        var playerOptional = entitiesDB.QueryUniqueEntityOptional<TransformComponent, RigidBodyComponent>(GameGroups.Player.BuildGroup);

        if (!playerOptional.HasValue)
            return;

        ref var transform = ref playerOptional.Get1();
        ref var rigidBody = ref playerOptional.Get2();

        if (_lastPosition == transform.Position)
            return;

        _lastPosition = transform.Position;
        
        new SyncPlayerPositionPacket(transform.Position, rigidBody.OnGround).SendToServer();
    }
}