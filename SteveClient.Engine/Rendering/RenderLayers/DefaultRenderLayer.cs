﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class DefaultRenderLayer<TVertex> : BaseRenderLayer where TVertex : IVertex
{
    private readonly VertexDefinitions.VertexDefinition<TVertex> _definition;
    private readonly int _elementBufferObject;
    private readonly int _vertexBufferObject;
    private readonly int _vertexArrayObject;

    private readonly List<BakedRenderData> _bakedModels = new();

    private int _elements;

    public DefaultRenderLayer(VertexDefinitions.VertexDefinition<TVertex> vertexDefinition)
    {
        _definition = vertexDefinition;
        
        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();

        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        _definition.Shader.Use();
    }

    public override Shader Shader => _definition.Shader;

    public override VertexFactory GetVertexFactory() => GetFactory(_definition.VertexType);

    public override void Upload(float[] vertices, uint[] indices)
    {
        _elements = indices.Length;
        
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.DynamicDraw);
    }

    public override void UploadBakedModel(BakedRenderData renderData)
    {
        _bakedModels.Add(renderData);
    }

    public override void RebuildBuffers()
    {
        _elements = 0;
        
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, BufferSize * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, BufferSize * sizeof(uint), IntPtr.Zero, BufferUsageHint.DynamicDraw);

        int vertexOffset = 0;
        int indexOffset = 0;

        foreach (var bakedModel in _bakedModels)
        {
            _elements += bakedModel.Indices.Length;
            
            var vertexSize = bakedModel.SizeOfVertices;
            var indexSize = bakedModel.SizeOfIndices;
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)vertexOffset, vertexSize, bakedModel.Vertices);
            
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)indexOffset, indexSize, bakedModel.Indices);

            vertexOffset += vertexSize;
            indexOffset += indexSize;
        }
    }

    public override void Bind()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        _definition.Shader.Use();
    }

    public override void BeforeRender()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.CullFace(CullFaceMode.Back);
    }

    public override void Render()
    {
        int offset = 0;
        
        foreach (var bakedModel in _bakedModels)
        {
            _definition.Shader.SetMatrix4("model", bakedModel.Model);
            _definition.Shader.SetMatrix4("view", CameraState.ViewMatrix);
            _definition.Shader.SetMatrix4("projection", CameraState.ProjectionMatrix);
            
            GL.DrawElements(_definition.PrimitiveType, bakedModel.Indices.Length, DrawElementsType.UnsignedInt, offset);

            offset += bakedModel.SizeOfIndices;
        }
    }

    public override void AfterRender()
    {
        GL.Disable(EnableCap.DepthTest);
    }

    public override void Flush()
    {
        _bakedModels.Clear();
    }
}