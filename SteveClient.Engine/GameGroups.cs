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

    // Groups
    
    
    // Exclusive Groups / Singleton Entities
    public abstract class MainCamera : GroupCompound<SimpleRigidBodies, Cameras, Controllable> { }
    public abstract class Player : GroupCompound<MinecraftEntities, ModelFilters> { }
}