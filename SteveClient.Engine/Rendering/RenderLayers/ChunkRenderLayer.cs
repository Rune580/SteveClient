using System.Collections.Concurrent;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Game;
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
    private readonly VertexDefinition<BlockVertex> _definition;
    private readonly Shader _shader;
    private readonly TargetSpace _space;

    private readonly ConcurrentQueue<BakedChunkSection> _chunkQueue = new();
    private readonly Dictionary<Vector3i, int> _posToChunkMap = new();
    private readonly List<BakedChunkPointer> _chunks = new();
    
    private int _verticesOffset;
    private int _indicesOffset;
    
    public ChunkRenderLayer(Shader shader)
    {
        _definition = BlockTriangles;
        _shader = shader;
        _space = TargetSpace.WorldSpace;
        
        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();
        
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, 10000000 * _definition.VertexSize * sizeof(float), IntPtr.Zero, BufferUsageHint.DynamicDraw);
        
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

    public void ClearChunks()
    {
        _chunks.Clear();
        _posToChunkMap.Clear();
        _chunkQueue.Clear();
        _verticesOffset = 0;
        _indicesOffset = 0;
    }

    public void RemoveChunk()
    {
        
    }

    public override void RebuildBuffers()
    {
        bool fullRebuild = false;
        
        while (!_chunkQueue.IsEmpty)
        {
            if (!_chunkQueue.TryDequeue(out var bakedSection))
                return;

            if (_posToChunkMap.TryGetValue(bakedSection.ChunkPos, out int i))
            {
                fullRebuild = true;
                _chunks.RemoveAt(i);
                _posToChunkMap.Remove(bakedSection.ChunkPos);
            }

            AddChunk(bakedSection);
        }

        if (!fullRebuild)
            return;

        _verticesOffset = 0;
        _indicesOffset = 0;

        var bakedSections = _chunks.Select(chunkPointer => chunkPointer.BakedSection).ToArray();
        _chunks.Clear();

        foreach (var bakedChunkSection in bakedSections)
            AddChunk(bakedChunkSection);
    }

    private void AddChunk(BakedChunkSection bakedSection)
    {
        int vertexOffset = _verticesOffset;
        int vertexLength = bakedSection.Vertices.Length * sizeof(float);

        int indexOffset = _indicesOffset;
        int indexLength = bakedSection.Indices.Length * sizeof(uint);
        
        BakedChunkPointer chunkPointer = new BakedChunkPointer(bakedSection,
            vertexOffset,
            vertexLength,
            indexOffset,
            indexLength);
            
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)_verticesOffset, vertexLength, chunkPointer.Vertices);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)_indicesOffset, indexLength, chunkPointer.Indices);

        _posToChunkMap[bakedSection.ChunkPos] = _chunks.Count;
        _chunks.Add(chunkPointer);

        _verticesOffset += vertexLength;
        _indicesOffset += indexLength;
    }

    public override void Bind()
    {
        GL.BindVertexArray(_vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        
        Shader.Use();
        Shader.SetInt("textureSampler", 0);
        Shader.SetInt("skyLightTex", 1);
        Shader.SetInt("blockLightTex", 2);
        
        TextureRegistry.BlockTextureAtlas.Use();
        
        if (World.ServerWorld is null)
            return;
        
        LightMap lightMap = World.ServerWorld.LightMap;
        lightMap.Use(1);
        
        Shader.SetInt("lightMapWidth", lightMap.Width);
        Shader.SetInt("lightMapHeight", lightMap.Height);
    }
    
    public override void BeforeRender()
    {
        GL.Enable(EnableCap.DepthTest);
        
        GL.Enable(EnableCap.CullFace);
        GL.CullFace(CullFaceMode.Front);
    }
    
    public override void Render()
    {
        Shader.SetVector3("viewPos", CameraState.Position);
        Shader.SetVector3("lightPos", new Vector3(-254, 90, 96));
        
        Shader.SetVector3("props.ambientStrength", new Vector3(0.4f));
        Shader.SetVector3("props.diffuseStrength", new Vector3(1f));
        Shader.SetVector3("props.specularStrength", new Vector3(0.0f));
        Shader.SetFloat("props.shininess", 32f);

        Shader.SetVector3("directionalLight.direction", new Vector3(1, -1, 1));
        Shader.SetVector3("directionalLight.color", Vector3.One);

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
        
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
    }

    public override void AfterRender()
    {
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.CullFace);
    }
    
    private readonly struct BakedChunkPointer : IEquatable<BakedChunkPointer>
    {
        public readonly BakedChunkSection BakedSection;
        public readonly int VerticesOffset;
        public readonly int VerticesLength;
        public readonly int IndicesOffset;
        public readonly int IndicesLength;

        public BakedChunkPointer(BakedChunkSection bakedSection, int verticesOffset, int verticesLength, int indicesOffset, int indicesLength)
        {
            BakedSection = bakedSection;
            VerticesOffset = verticesOffset;
            VerticesLength = verticesLength;
            IndicesOffset = indicesOffset;
            IndicesLength = indicesLength;
        }

        public float[] Vertices => BakedSection.Vertices;
        public uint[] Indices => BakedSection.Indices;
        public int IndicesCount => BakedSection.Indices.Length;
        public Matrix4 Model => BakedSection.Transform;

        public bool Equals(BakedChunkPointer other)
        {
            return BakedSection.ChunkPos == other.BakedSection.ChunkPos;
        }
        
        public override bool Equals(object? obj)
        {
            return obj is BakedChunkPointer other && Equals(other);
        }

        public override int GetHashCode()
        {
            return BakedSection.ChunkPos.GetHashCode();
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