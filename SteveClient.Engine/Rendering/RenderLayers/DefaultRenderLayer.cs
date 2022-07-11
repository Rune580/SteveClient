using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.VertexDefinitions;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class DefaultRenderLayer<TVertex> : BaseRenderLayer, IBakedRenderDataHandler
    where TVertex : IVertex
{
    private readonly int _elementBufferObject;
    private readonly int _vertexBufferObject;
    private readonly int _vertexArrayObject;
    private readonly Shader _shader;
    
    protected readonly VertexDefinition<TVertex> Definition;
    protected readonly TargetSpace Space;
    protected readonly List<IBakedRenderData> RenderData = new();

    private uint _elements;

    public DefaultRenderLayer(VertexDefinition<TVertex> vertexDefinition, Shader shader, TargetSpace targetSpace)
    {
        Definition = vertexDefinition;
        _shader = shader;
        Space = targetSpace;
        
        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        _shader.Use();
    }

    public override Shader Shader => _shader;

    public VertexFactory GetVertexFactory() => GetFactory(Definition.VertexType);

    public void UploadRenderData(IBakedRenderData renderData)
    {
        for (int i = 0; i < renderData.Indices.Length; i++)
            renderData.Indices[i] =_elements + renderData.Indices[i];
        
        RenderData.Add(renderData);
        _elements += (uint)(renderData.Vertices.Length / Definition.VertexSize);
    }

    public override void RebuildBuffers()
    {
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, BufferSize * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, BufferSize * sizeof(uint), IntPtr.Zero, BufferUsageHint.DynamicDraw);

        int vertexOffset = 0;
        int indexOffset = 0;

        foreach (var renderData in RenderData)
        {
            var vertexSize = renderData.SizeOfVertices;
            var indexSize = renderData.SizeOfIndices;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)vertexOffset, vertexSize, renderData.Vertices);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)indexOffset, indexSize, renderData.Indices);

            vertexOffset += vertexSize;
            indexOffset += indexSize;
        }
    }

    public override void Bind()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        Shader.Use();
    }

    public override void BeforeRender()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.CullFace(CullFaceMode.Back);
    }

    public override void Render()
    {
        int offset = 0;
        
        foreach (var bakedModel in RenderData)
        {
            if (bakedModel.HasTexture)
                bakedModel.UseTexture();

            if (bakedModel.HasShaderProperties)
                bakedModel.ApplyShaderProperties(Shader);
            
            Shader.SetMatrix4("model", bakedModel.Transform);
            Shader.SetMatrix4("view", Space.ViewMatrix);
            Shader.SetMatrix4("projection", Space.ProjectionMatrix);
            
            GL.DrawElements(Definition.PrimitiveType, bakedModel.Indices.Length, DrawElementsType.UnsignedInt, offset);

            offset += bakedModel.SizeOfIndices;
        }
    }

    public override void AfterRender()
    {
        GL.Disable(EnableCap.DepthTest);
    }

    public override void Flush()
    {
        RenderData.Clear();
        _elements = 0;
    }
}