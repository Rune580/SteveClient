using SteveClient.Engine.Components;
using Svelto.ECS;

namespace SteveClient.Engine.Descriptors;

public class PlayerDescriptor : ExtendibleEntityDescriptor<MinecraftEntityDescriptor>
{
    public PlayerDescriptor() : base(new IComponentBuilder[]
    {
        new ComponentBuilder<PlayerComponent>()
    }) { }
}