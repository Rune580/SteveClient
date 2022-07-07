using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.Font;
using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.Definitions.VertexDefinitions;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class FontRenderLayer : BaseRenderLayer
{
    private readonly VertexDefinition<PositionTexture> _definition = DefaultFont;
    private readonly int _elementBufferObject;
    private readonly int _vertexBufferObject;
    private readonly int _vertexArrayObject;

    private readonly List<BakedTextRender> _textRenders = new();
    private readonly float[] _vertices;
    private readonly uint[] _indices;
    private readonly RenderTarget _renderTarget;

    public FontRenderLayer(RenderTarget renderTarget)
    {
        _renderTarget = renderTarget;
        
        _vertices = new VertexDataArray(PositionTexture.Size * 4)
            .WithVector3(1f, 1f, 0).WithVector2(1, 0)
            .WithVector3(1f, 0f, 0).WithVector2(1, 1)
            .WithVector3(0f, 0f, 0).WithVector2(0, 1)
            .WithVector3(0f, 1f, 0).WithVector2(0, 0);

        _indices = new uint[]
        {
            0, 1, 3,
            1, 2, 3
        };

        _vertexArrayObject = GL.GenVertexArray();
        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();
        
        GL.BindVertexArray(_vertexArrayObject);
        
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, PositionTexture.Stride * 4, _vertices, BufferUsageHint.StaticDraw);
        
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * 6, _indices, BufferUsageHint.StaticDraw);
        
        _definition.Shader.Use();
    }

    public override Shader Shader => Wireframe ? ShaderDefinitions.PosTexWireframeShader : _definition.Shader;

    public void Upload(BakedTextRender textRender)
    {
        _textRenders.Add(textRender);
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
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public override void Render()
    {
        foreach (var textRender in _textRenders)
        {
            for (int i = 0; i < textRender.Characters.Length; i++)
            {
                Character character = textRender.Characters[i];
                Matrix4 transform = textRender.Transforms[i];
                Color4 color = textRender.Colors[i];
                
                character.Bind();
                
                Shader.SetMatrix4("model", transform);
                Shader.SetMatrix4("view", _renderTarget.ViewMatrix);
                Shader.SetMatrix4("projection", _renderTarget.ProjectionMatrix);
                Shader.SetColor("color", color);
                
                GL.DrawElements(_definition.PrimitiveType, _indices.Length, DrawElementsType.UnsignedInt, 0);
            }
        }
    }

    public override void AfterRender()
    {
        GL.Disable(EnableCap.DepthTest);
        GL.Disable(EnableCap.Blend);
    }

    public override void Flush()
    {
        _textRenders.Clear();
    }
}