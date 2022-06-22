using SteveClient.Engine.Components;
using Svelto.ECS;

namespace SteveClient.Engine.Descriptors;

public class ControllableCameraDescriptor : ExtendibleEntityDescriptor<CameraDescriptor>
{
    public ControllableCameraDescriptor() : base(new IComponentBuilder[] { new ComponentBuilder<CameraControllerComponent>()}) { }
}