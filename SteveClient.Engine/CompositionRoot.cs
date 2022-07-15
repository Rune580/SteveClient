using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.AssetManagement.ModelLoading;
using SteveClient.Engine.Components;
using SteveClient.Engine.Descriptors;
using SteveClient.Engine.Engines;
using SteveClient.Engine.Engines.PacketProcessing;
using SteveClient.Engine.Engines.Rendering;
using SteveClient.Engine.Engines.ServerWorld;
using SteveClient.Engine.Engines.Tools;
using SteveClient.Engine.Game;
using SteveClient.Engine.Networking;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Minecraft.Data;
using SteveClient.Minecraft.DataGen;
using Svelto.ECS;
using Svelto.ECS.Schedulers;

namespace SteveClient.Engine;

public class CompositionRoot
{
    private SimpleEntitiesSubmissionScheduler _submissionScheduler;

    public EngineScheduler Scheduler { get; }
    public EngineScheduler GraphicsScheduler { get; }

    public CompositionRoot()
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
        var blockUpdateChunkSectionEngine = new BlockUpdateChunkSectionEngine(world);
        var moveEntityEngine = new MoveEntityEngine(world);
        var moveAndRotateEntityEngine = new MoveAndRotateEntityEngine(world);
        var teleportEntityEngine = new TeleportEntityEngine(world);
        var spawnBlockEntityEngine = new SpawnBlockModelEntityEngine(entityFactory);
        
        // Create render engines
        var renderModelFiltersEngine = new RenderModelFiltersEngine();
        var renderBlockEntitiesEngine = new RenderBlockStateEntityEngine();
        var renderChunkSectionsEngine = new RenderChunkSectionsEngine(world);
        
        // Add engines
        enginesRoot.AddEngine(applyVelocityToSimpleRigidBodiesEngine);
        enginesRoot.AddEngine(cameraControlsEngine);
        enginesRoot.AddEngine(spawnPlayerEntityEngine);
        enginesRoot.AddEngine(teleportPlayerEntityEngine);
        enginesRoot.AddEngine(spawnOnlineEntityEngine);
        enginesRoot.AddEngine(spawnChunkSectionEntityEngine);
        enginesRoot.AddEngine(blockUpdateChunkSectionEngine);
        enginesRoot.AddEngine(moveEntityEngine);
        enginesRoot.AddEngine(moveAndRotateEntityEngine);
        enginesRoot.AddEngine(teleportEntityEngine);
        enginesRoot.AddEngine(spawnBlockEntityEngine);
        
        // Add render engines
        enginesRoot.AddEngine(updateCameraStateFromCamerasEngine);
        enginesRoot.AddEngine(renderModelFiltersEngine);
        enginesRoot.AddEngine(renderBlockEntitiesEngine);
        enginesRoot.AddEngine(renderChunkSectionsEngine);
        
        // Register scheduled engines
        Scheduler.RegisterScheduledEngine(applyVelocityToSimpleRigidBodiesEngine);
        Scheduler.RegisterScheduledEngine(cameraControlsEngine);
        Scheduler.RegisterScheduledEngine(spawnPlayerEntityEngine);
        Scheduler.RegisterScheduledEngine(teleportPlayerEntityEngine);
        Scheduler.RegisterScheduledEngine(spawnOnlineEntityEngine);
        Scheduler.RegisterScheduledEngine(spawnChunkSectionEntityEngine);
        Scheduler.RegisterScheduledEngine(blockUpdateChunkSectionEngine);
        Scheduler.RegisterScheduledEngine(moveEntityEngine);
        Scheduler.RegisterScheduledEngine(moveAndRotateEntityEngine);
        Scheduler.RegisterScheduledEngine(teleportEntityEngine);
        Scheduler.RegisterScheduledEngine(spawnBlockEntityEngine);

        GraphicsScheduler.RegisterScheduledEngine(updateCameraStateFromCamerasEngine);
        GraphicsScheduler.RegisterScheduledEngine(renderModelFiltersEngine);
        GraphicsScheduler.RegisterScheduledEngine(renderBlockEntitiesEngine);
        GraphicsScheduler.RegisterScheduledEngine(renderChunkSectionsEngine);
        
        BuildCamera(entityFactory);
    }

    private void LoadMinecraftData()
    {
        DataGenerator.GenerateData();
        TextureRegistry.InitBlockTextureAtlas();
        BlockModelLoader.LoadBlockModels();
    }

    private void BuildCamera(IEntityFactory entityFactory)
    {
        EntityInitializer initializer =
            entityFactory.BuildEntity<ControllableCameraDescriptor>(Egid.Camera,
                GameGroups.MainCamera.BuildGroup);
        
        initializer.Init(new TransformComponent());
        initializer.Init(new SimpleRigidBodyComponent());
        initializer.Init(new CameraComponent());
        initializer.Init(new CameraControllerComponent(1.5f));
    }
}