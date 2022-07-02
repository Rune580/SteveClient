using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.VertexData;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Engine.Rendering.Builders;

public class RenderBuilder
{
    private readonly List<Vector3> _vertices;
    private readonly List<Vector3> _normals;
    private readonly List<Color4> _colors;
    private readonly List<Vector2> _uvs;
    private readonly List<uint> _indices;

    private Matrix4 _model;
    private Matrix4 _view;
    private Matrix4 _projection;
    private Color4 _currentColor;

    public RenderBuilder()
    {
        _vertices = new List<Vector3>();
        _normals = new List<Vector3>();
        _colors = new List<Color4>();
        _uvs = new List<Vector2>();
        _indices = new List<uint>();
        
        _model = Matrix4.CreateTranslation(Vector3.Zero);
        _currentColor = Color4.White;
    }

    public void Upload(BaseRenderLayer renderLayer)
    {
        IVertex[] vertices = renderLayer.GetVertexFactory().Consume(_vertices.ToArray(), _normals.ToArray(), _colors.ToArray(), _uvs.ToArray());

        BakedRenderData bakedRenderData = new BakedRenderData(vertices.VertexData(), _indices.ToArray(), _model);
  
        renderLayer.UploadRenderData(bakedRenderData);
        
        _vertices.Clear();
        _normals.Clear();
        _colors.Clear();
        _uvs.Clear();
        _indices.Clear();
    }

    public RenderBuilder WithModel(Matrix4 model)
    {
        _model = model;

        return this;
    }

    public RenderBuilder WithCamera(Matrix4 view, Matrix4 projection)
    {
        _view = view;
        _projection = projection;

        return this;
    }

    public RenderBuilder WithTransform(ref TransformComponent transform)
    {
        return Rotate(transform.Rotation)
            .Translate(transform.Position);
        // return Translate(transform.Position)
        //     .Rotate(transform.Rotation);
    }

    public RenderBuilder Translate(Vector3 translation)
    {
        _model += Matrix4.CreateTranslation(translation);

        return this;
    }

    public RenderBuilder Rotate(Quaternion rotation)
    {
        _model *= Matrix4.CreateFromQuaternion(rotation);

        return this;
    }

    public RenderBuilder Scale(Vector3 scale)
    {
        _model *= Matrix4.CreateScale(scale);

        return this;
    }

    public RenderBuilder WithColor(Color4 color)
    {
        _currentColor = color;

        return this;
    }

    public RenderBuilder WithMesh(SimpleModel model)
    {
        for (int i = 0; i < model.Vertices.Length; i++)
            _colors.Add(_currentColor);
        
        _vertices.AddRange(model.Vertices);
        _indices.AddRange(model.Indices);

        return this;
    }
}