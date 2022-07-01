using Svelto.ECS;

namespace SteveClient.Engine;

public static class GameGroups
{
    // Tags
    public abstract class SimpleRigidBodies : GroupTag<SimpleRigidBodies> { }
    public abstract class Cameras : GroupTag<Cameras> { }
    public abstract class Controllable : GroupTag<Controllable> { }
    public abstract class ModelFilters : GroupTag<ModelFilters> { }
    public abstract class Entities : GroupTag<Entities> { }
    public abstract class PlayerTag : GroupTag<PlayerTag> { }
    public abstract class WorldBlocks : GroupTag<WorldBlocks> { }

    // Groups
    public abstract class MinecraftEntities : GroupCompound<Entities, ModelFilters> { }

    // Exclusive Groups / Singleton Entities
    public abstract class MainCamera : GroupCompound<SimpleRigidBodies, Cameras, Controllable> { }
    public abstract class Player : GroupCompound<PlayerTag, Entities, ModelFilters> { }
    public abstract class ChunkSections : GroupTag<ChunkSections> { }
}