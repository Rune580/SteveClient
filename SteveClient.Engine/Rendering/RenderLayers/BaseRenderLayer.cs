using OpenTK.Graphics.OpenGL4;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.Shaders;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public abstract class BaseRenderLayer
{
    public const int BufferSize = ushort.MaxValue;

    protected PolygonMode DefaultPolygonMode { get; set; } = PolygonMode.Fill; 
    public bool Wireframe { get; set; }
    
    protected BaseRenderLayer()
    {
        RenderLayerDefinitions.Instances.Add(this);
    }

    public abstract Shader Shader { get; }

    public virtual void RebuildBuffers() { }

    public void PreRender()
    {
        GL.PolygonMode(MaterialFace.FrontAndBack, Wireframe ? PolygonMode.Line : DefaultPolygonMode);

        BeforeRender();
    }

    public void PostRender()
    {
        AfterRender();
        
        GL.PolygonMode(MaterialFace.FrontAndBack, DefaultPolygonMode);
    }
    
    public virtual void BeforeRender() { }

    public abstract void Bind();
    
    public abstract void Render();
    
    public virtual void AfterRender() { }
    
    public virtual void Flush() { }
}