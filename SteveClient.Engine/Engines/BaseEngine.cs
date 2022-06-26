using Svelto.ECS;

namespace SteveClient.Engine.Engines;

public abstract class BaseEngine : IQueryingEntitiesEngine, IScheduledEngine 
{
    public virtual void Ready()
    {
        
    }
    
    public EntitiesDB entitiesDB { get; set; }
    
    public abstract void Execute(float delta);
}