using OpenTK.Mathematics;
using SteveClient.Engine.Components;
using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Models;
using SteveClient.Engine.Rendering.RenderLayers;
using SteveClient.Engine.Rendering.UnBaked;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.Utils;

public class BlockRenderHelper
{
    private readonly List<Color4> _colors;
    private readonly List<TexturedQuad> _quads;

    private Color4 _currentColor;
    private Matrix4 _transform;

    public BlockRenderHelper()
    {
        _colors = new List<Color4>();
        _quads = new List<TexturedQuad>();
        
        _currentColor = Color4.White;
    }

    public void Upload<TRenderLayer>(TRenderLayer renderLayer)
        where TRenderLayer : BaseRenderLayer, IBakedRenderDataHandler
    {
        var factory = renderLayer.GetVertexFactory();
        
        Color4[] colors = { _currentColor, _currentColor, _currentColor, _currentColor };

        for (int i = 0; i < _quads.Count; i++)
        {
            IVertex[] vertices = factory.Consume(_quads[i].Vertices, Array.Empty<Vector3>(), colors, _quads[i].Uvs);
            
            BakedQuad bakedQuad = new BakedQuad(vertices.VertexData(), _quads[i].Triangles, _quads[i].TextureResourceName, _transform);

            renderLayer.UploadRenderData(bakedQuad);
        }
        
        _quads.Clear();
    }

    public BlockRenderHelper WithColor(Color4 color)
    {
        _currentColor = color;

        return this;
    }

    public BlockRenderHelper WithTransform(ref TransformComponent transform)
    {
        return Rotate(transform.Rotation)
            .Translate(transform.Position);
    }
    
    public BlockRenderHelper Translate(Vector3 translation)
    {
        _transform += Matrix4.CreateTranslation(translation);

        return this;
    }

    public BlockRenderHelper Rotate(Quaternion rotation)
    {
        _transform *= Matrix4.CreateFromQuaternion(rotation);

        return this;
    }

    public BlockRenderHelper Scale(Vector3 scale)
    {
        _transform *= Matrix4.CreateScale(scale);

        return this;
    }

    public BlockRenderHelper WithBlockModel(BlockModel blockModel)
    {
        foreach (var quad in blockModel.Quads)
        {
            Vector3[] vertices =
            {
                blockModel.Vertices[quad.Vertices[0]],
                blockModel.Vertices[quad.Vertices[1]],
                blockModel.Vertices[quad.Vertices[2]],
                blockModel.Vertices[quad.Vertices[3]]
            };
            
            TexturedQuad texturedQuad = new TexturedQuad(vertices, quad.Uvs, quad.TextureResourceName);
            
            _quads.Add(texturedQuad);
        }

        return this;
    }
}