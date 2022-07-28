using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Utils;
using Svelto.ECS;

namespace SteveClient.Engine.Engines.Rendering;

public class RenderEntityLookDirEngine : RenderingEngine
{
    public override void Execute(float delta)
    {
        foreach (var ((transforms, heads, count), _) in entitiesDB.QueryEntities<TransformComponent, HeadComponent>(GameGroups.MinecraftEntities.Groups))
        {
            for (int i = 0; i < count; i++)
            {
                ref var transform = ref transforms[i];
                ref var head = ref heads[i];

                Vector3 headLocation = transform.Position + new Vector3(0, 1.5f, 0);

                Vector3 lookDir = transform.Rotation * head.GetForward();
                Vector3 lookEnd = headLocation + (lookDir * 2);

                LineRenderer.DrawLine(headLocation, lookEnd, Color4.Red);
            }
        }
    }
}