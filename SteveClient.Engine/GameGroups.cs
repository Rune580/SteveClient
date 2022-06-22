using Svelto.ECS;

namespace SteveClient.Engine;

public static class GameGroups
{
    // Tags
    public abstract class SimpleRigidBodies : GroupTag<SimpleRigidBodies> { }
    public abstract class Cameras : GroupTag<Cameras> { }
    public abstract class Controllable : GroupTag<Controllable> { }
    public abstract class ModelFilters : GroupTag<ModelFilters> { }

    // Groups
    public abstract class ControllableCameras : GroupCompound<SimpleRigidBodies, Cameras, Controllable> { }
}