using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct ModelFilterComponent : IEntityComponent
{
    public int ModelIndex;

    public ModelFilterComponent(int modelIndex)
    {
        ModelIndex = modelIndex;
    }
    
    public ModelFilterComponent(SimpleInternalModel internalModel) : this(internalModel.Index) { }
}