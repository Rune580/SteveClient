using SteveClient.Engine.Engines;
using Svelto.ECS.Schedulers;

namespace SteveClient.Engine;

public class EngineScheduler
{
    private readonly SimpleEntitiesSubmissionScheduler _submissionScheduler;
    private readonly List<IScheduledEngine> _scheduledEngines;

    public EngineScheduler(SimpleEntitiesSubmissionScheduler submissionScheduler)
    {
        _submissionScheduler = submissionScheduler;
        _scheduledEngines = new List<IScheduledEngine>();
    }

    public void Execute(float delta)
    {
        foreach (var engine in _scheduledEngines)
            engine.Execute(delta);
        
        _submissionScheduler.SubmitEntities();
    }

    public void RegisterScheduledEngine(IScheduledEngine scheduledEngine)
    {
        _scheduledEngines.Add(scheduledEngine);
    }
}