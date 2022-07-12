using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.Shaders;
using SteveClient.Engine.Rendering.Utils.ChunkSections;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.VertexDefinitions;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class ChunkRenderLayer : BaseRenderLayer
{
    private readonly int _elementBufferObject;
    private readonly int _vertexBufferObject;
    private readonly int _vertexArrayObject;
    private readonly VertexDefinition<PositionTextureAtlas> _definition;
    private readonly Shader _shader;
    private readonly TargetSpace _space;

    private readonly ConcurrentQueue<BakedChunkSection> _chunkQueue = new();
    private readonly List<BakedChunkPointer> _chunks = new();

    private int _verticesOffset;
    private int _indicesOffset;
    
    public ChunkRenderLayer(Shader shader)
    {
        _definition = PositionTextureAtlasTriangles;
        _shader = shader;
        _space = TargetSpace.WorldSpace;
        
        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();
        
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, 100000000 * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, 100000000 * sizeof(uint), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        
        _shader.Use();

        _verticesOffset = 0;
        _indicesOffset = 0;
    }

    public override Shader Shader => _shader;

    public void UploadChunk(BakedChunkSection bakedSection)
    {
        _chunkQueue.Enqueue(bakedSection);
    }

    public override void RebuildBuffers()
    {
        while (!_chunkQueue.IsEmpty)
        {
            if (!_chunkQueue.TryDequeue(out var bakedSection))
                return;
            
            int vertexOffset = _verticesOffset;
            int vertexLength = bakedSection.Vertices.Length * sizeof(float);

            int indexOffset = _indicesOffset;
            int indexLength = bakedSection.Indices.Length * sizeof(uint);
        
            BakedChunkPointer chunkPointer = new BakedChunkPointer(bakedSection.Vertices,
                vertexOffset,
                vertexLength,
                bakedSection.Indices,
                bakedSection.Indices.Length,
                indexOffset,
                indexLength,
                bakedSection.Hash,
                bakedSection.Transform);
        
            Bind();
        
            GL.BindVertexArray(_vertexArrayObject);
        
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)_verticesOffset, vertexLength, chunkPointer.Vertices);
        
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)_indicesOffset, indexLength, chunkPointer.Indices);

            _chunks.Add(chunkPointer);

            _verticesOffset += vertexLength;
            _indicesOffset += indexLength;
        }
    }

    public override void Bind()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        Shader.Use();
        TextureRegistry.BlockTextureAtlas.Use();
    }
    
    public override void BeforeRender()
    {
        GL.Enable(EnableCap.DepthTest);
        GL.CullFace(CullFaceMode.Back);
    }
    
    public override void Render()
    {
        Bind();
        
        Shader.SetColor("tint", Color4.White);
        Shader.SetMatrix4("view", _space.ViewMatrix);
        Shader.SetMatrix4("projection", _space.ProjectionMatrix);

        int baseVertex = 0;

        foreach (var chunkPointer in _chunks)
        {
            Shader.SetMatrix4("model", chunkPointer.Model);
            
            GL.DrawElementsBaseVertex(_definition.PrimitiveType, chunkPointer.IndicesCount, DrawElementsType.UnsignedInt, (IntPtr)chunkPointer.IndicesOffset, baseVertex);

            baseVertex += chunkPointer.Vertices.Length / _definition.VertexSize;
        }
    }

    public override void AfterRender()
    {
        GL.Disable(EnableCap.DepthTest);
    }
    
    private readonly struct BakedChunkPointer : IEquatable<BakedChunkPointer>
    {
        public readonly float[] Vertices;
        public readonly int VerticesOffset;
        public readonly int VerticesLength;
        public readonly uint[] Indices;
        public readonly int IndicesCount;
        public readonly int IndicesOffset;
        public readonly int IndicesLength;
        public readonly long Hash;
        public readonly Matrix4 Model;

        public BakedChunkPointer(float[] vertices, int verticesOffset, int verticesLength, uint[] indices, int indicesCount, int indicesOffset, int indicesLength, long hash, Matrix4 model)
        {
            Vertices = vertices;
            VerticesOffset = verticesOffset;
            VerticesLength = verticesLength;
            Indices = indices;
            IndicesCount = indicesCount;
            IndicesOffset = indicesOffset;
            IndicesLength = indicesLength;
            Hash = hash;
            Model = model;
        }

        public bool Equals(BakedChunkPointer other)
        {
            return Hash == other.Hash;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is BakedChunkPointer other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Hash.GetHashCode();
        }

        public static bool operator ==(BakedChunkPointer left, BakedChunkPointer right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BakedChunkPointer left, BakedChunkPointer right)
        {
            return !left.Equals(right);
        }
    }
}