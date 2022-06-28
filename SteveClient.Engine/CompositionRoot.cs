using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Engines;
using SteveClient.Engine.Engines.PacketProcessing;
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

        var world = new World();

        // Create engines
        var applyVelocityToSimpleRigidBodiesEngine = new ApplyVelocityToSimpleRigidBodiesEngine();
        var cameraControlsEngine = new CameraControlsEngine();
        var updateCameraStateFromCamerasEngine = new UpdateCameraStateFromCamerasEngine();
        var spawnPlayerEntityEngine = new SpawnPlayerEntityEngine(entityFactory);
        var teleportPlayerEntityEngine = new TeleportPlayerEntityEngine();
        var spawnOnlineEntityEngine = new SpawnOnlinePlayerEntityEngine(entityFactory, world);
        var spawnChunkSectionEntityEngine = new SpawnChunkSectionEntityEngine(entityFactory, world);
        var moveEntityEngine = new MoveEntityEngine(world);
        var moveAndRotateEntityEngine = new MoveAndRotateEntityEngine(world);
        
        // Create render engines
        var renderModelFiltersEngine = new RenderModelFiltersEngine();
        
        // Add engines
        enginesRoot.AddEngine(applyVelocityToSimpleRigidBodiesEngine);
        enginesRoot.AddEngine(cameraControlsEngine);
        enginesRoot.AddEngine(spawnPlayerEntityEngine);
        enginesRoot.AddEngine(teleportPlayerEntityEngine);
        enginesRoot.AddEngine(spawnOnlineEntityEngine);
        enginesRoot.AddEngine(spawnChunkSectionEntityEngine);
        enginesRoot.AddEngine(moveEntityEngine);
        enginesRoot.AddEngine(moveAndRotateEntityEngine);
        
        // Add render engines
        enginesRoot.AddEngine(updateCameraStateFromCamerasEngine);
        enginesRoot.AddEngine(renderModelFiltersEngine);
        
        // Register scheduled engines
        Scheduler.RegisterScheduledEngine(applyVelocityToSimpleRigidBodiesEngine);
        Scheduler.RegisterScheduledEngine(cameraControlsEngine);
        Scheduler.RegisterScheduledEngine(spawnPlayerEntityEngine);
        Scheduler.RegisterScheduledEngine(teleportPlayerEntityEngine);
        Scheduler.RegisterScheduledEngine(spawnOnlineEntityEngine);
        Scheduler.RegisterScheduledEngine(spawnChunkSectionEntityEngine);
        Scheduler.RegisterScheduledEngine(moveEntityEngine);
        Scheduler.RegisterScheduledEngine(moveAndRotateEntityEngine);

        GraphicsScheduler.RegisterScheduledEngine(updateCameraStateFromCamerasEngine);
        GraphicsScheduler.RegisterScheduledEngine(renderModelFiltersEngine);
        
        BuildCamera(entityFactory, clientSize);
    }

    private void LoadMinecraftData()
    {
        DataGenerator.GenerateData();
    }

    private void BuildCamera(IEntityFactory entityFactory, Vector2i clientSize)
    {
        EntityInitializer initializer =
            entityFactory.BuildEntity<ControllableCameraDescriptor>(Egid.Camera,
                GameGroups.MainCamera.BuildGroup);
        
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