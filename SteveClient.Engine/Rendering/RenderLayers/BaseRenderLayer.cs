using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public abstract class BaseRenderLayer
{
    public const int BufferSize = ushort.MaxValue;
    
    protected BaseRenderLayer()
    {
        RenderLayerDefinitions.Instances.Add(this);
    }

    public abstract Shader Shader { get; }

    public abstract VertexFactory GetVertexFactory();

    public abstract void RebuildBuffers();
    
    public virtual void BeforeRender() { }

    public abstract void Bind();
    
    public abstract void Render();
    
    public virtual void AfterRender() { }
    
    public virtual void Flush() { }
}