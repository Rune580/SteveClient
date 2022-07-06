using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering;
using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public class UpdateCameraStateFromCamerasEngine : IQueryingEntitiesEngine, IScheduledEngine
{
    public EntitiesDB entitiesDB { get; set; }
    
    public void Ready() { }
    
    public void Execute(float delta)
    {
        foreach (var ((transforms, cameras, count), _) in entitiesDB.QueryEntities<TransformComponent, CameraComponent>(GameGroups.MainCamera.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var camera = ref cameras[i];

                if (camera.IsActive)
                    CameraState.Update(camera.GetViewMatrix(transform.Position / 2),
                        camera.GetProjectionMatrix(),
                        camera.GetScreenSpaceMatrix(),
                        transform.Position, camera.Front, camera.Up, camera.Right);
            }
        }
    }
}