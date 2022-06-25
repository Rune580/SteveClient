using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Engines;
using SteveClient.Engine.Networking;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.DataGen;
using Svelto.ECS;
using Svelto.ECS.Schedulers;

namespace SteveClient.Engine;

public class CompositionRoot
{
    private SimpleEntitiesSubmissionScheduler _submissionScheduler;

    public EngineScheduler Scheduler { get; }
    public EngineScheduler GraphicsScheduler { get; }

    public CompositionRoot(Vector2i clientSize)
    {
        LoadMinecraftData();
        
        var client = new MinecraftNetworkingClient();
        
        _submissionScheduler = new SimpleEntitiesSubmissionScheduler();

        Scheduler = new EngineScheduler(_submissionScheduler);
        GraphicsScheduler = new EngineScheduler(_submissionScheduler);

        var enginesRoot = new EnginesRoot(_submissionScheduler);
        var entityFactory = enginesRoot.GenerateEntityFactory();
        var entityFunctions = enginesRoot.GenerateEntityFunctions();

        // Create engines
        var applyVelocityToSimpleRigidBodiesEngine = new ApplyVelocityToSimpleRigidBodiesEngine();
        var doMovementOnControllableCamerasEngine = new DoMovementOnControllableCamerasEngine();
        var updateCameraStateFromCamerasEngine = new UpdateCameraStateFromCamerasEngine();
        var renderModelFiltersEngine = new RenderModelFiltersEngine();
        
        // Add engines
        enginesRoot.AddEngine(applyVelocityToSimpleRigidBodiesEngine);
        enginesRoot.AddEngine(doMovementOnControllableCamerasEngine);
        enginesRoot.AddEngine(updateCameraStateFromCamerasEngine);
        enginesRoot.AddEngine(renderModelFiltersEngine);
        
        // Register scheduled engines
        Scheduler.RegisterScheduledEngine(applyVelocityToSimpleRigidBodiesEngine);
        Scheduler.RegisterScheduledEngine(doMovementOnControllableCamerasEngine);
        
        GraphicsScheduler.RegisterScheduledEngine(updateCameraStateFromCamerasEngine);
        GraphicsScheduler.RegisterScheduledEngine(renderModelFiltersEngine);
        
        BuildCamera(entityFactory, clientSize);
        
        AddBlock(entityFactory, new Vector3(10, 0, 0));
        AddBlock(entityFactory, new Vector3(-10, 0, 0));
        AddBlock(entityFactory, new Vector3(0, 0, 10));
        AddBlock(entityFactory, new Vector3(0, 0, -10));
    }

    private void LoadMinecraftData()
    {
        DataGenerator.GenerateData();
    }

    private void BuildCamera(IEntityFactory entityFactory, Vector2i clientSize)
    {
        EntityInitializer initializer =
            entityFactory.BuildEntity<ControllableCameraDescriptor>(Egid.Camera,
                GameGroups.ControllableCameras.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new SimpleRigidBodyComponent());
        initializer.Init(new CameraComponent(clientSize.X / (float)clientSize.Y));
        initializer.Init(new CameraControllerComponent(1.5f));
    }

    private void AddBlock(IEntityFactory entityFactory, Vector3 worldPos)
    {
        EntityInitializer initializer =
            entityFactory.BuildEntity<StaticBlockDescriptor>(Egid.NextId, GameGroups.ModelFilters.BuildGroup);
        
        initializer.Init(new TransformComponent(worldPos));
        initializer.Init(new ModelFilterComponent(Primitives.Cube.Index));
    }
}