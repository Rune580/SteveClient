using Svelto.ECS;

namespace SteveClient.Engine;

public static class GameGroups
{
    // Tags
    public abstract class SimpleRigidBodies : GroupTag<SimpleRigidBodies> { }
    public abstract class Cameras : GroupTag<Cameras> { }
    public abstract class Controllable : GroupTag<Controllable> { }
    public abstract class ModelFilters : GroupTag<ModelFilters> { }
    public abstract class MinecraftEntities : GroupTag<MinecraftEntities> { }
    public abstract class Players: GroupTag<Players> { }

    // Groups
    public abstract class ControllableCameras : GroupCompound<SimpleRigidBodies, Cameras, Controllable> { }
    public abstract class PlayerEntities : GroupCompound<ModelFilters, MinecraftEntities, Players> { }
}