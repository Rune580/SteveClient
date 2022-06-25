using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct MinecraftEntityComponent : IEntityComponent
{
    public int EntityId;

    public MinecraftEntityComponent(int entityId)
    {
        EntityId = entityId;
    }
}