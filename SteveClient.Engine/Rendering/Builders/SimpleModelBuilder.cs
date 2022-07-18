using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Models;

namespace SteveClient.Engine.Rendering.Builders;

public class SimpleModelBuilder
{
    private readonly List<Vector3> _vertices;
    private readonly List<Color4> _colors;
    private readonly List<uint> _indices;

    private Color4 _currentColor;

    public SimpleModelBuilder()
    {
        _vertices = new List<Vector3>();
        _colors = new List<Color4>();
        _indices = new List<uint>();
        
        _currentColor = Color4.White;
    }

    public SimpleInternalModel Build()
    {
        var vertices = _vertices.ToArray();
        var indices = _indices.ToArray();

        SimpleInternalModel result = new SimpleInternalModel(vertices, indices);
        
        _vertices.Clear();
        _indices.Clear();

        return result;
    }

    public SimpleModelBuilder WithColor(Color4 color)
    {
        _currentColor = color;
        return this;
    }

    private SimpleModelBuilder AddVertex(Vector3 vertex, Color4 color)
    {
        _vertices.Add(vertex);
        _colors.Add(color);
        
        return this;
    }

    private SimpleModelBuilder AddVertex(Vector3 vertex)
    {
        return AddVertex(vertex, _currentColor);
    }

    public SimpleModelBuilder AddQuad(Vector3 topLeft, Vector3 topRight, Vector3 botRight, Vector3 botLeft)
    {
        int itr = _vertices.IndexOf(topRight);
        int ibr = _vertices.IndexOf(botRight);
        int ibl = _vertices.IndexOf(botLeft);
        int itl = _vertices.IndexOf(topLeft);

        if (itr < 0)
        {
            AddVertex(topRight);
            itr = _vertices.Count - 1;
        }

        if (ibr < 0)
        {
            AddVertex(botRight);
            ibr = _vertices.Count - 1;
        }

        if (ibl < 0)
        {
            AddVertex(botLeft);
            ibl = _vertices.Count - 1;
        }

        if (itl < 0)
        {
            AddVertex(topLeft);
            itl = _vertices.Count - 1;
        }

        _indices.Add((uint)itr);
        _indices.Add((uint)ibr);
        _indices.Add((uint)itl);
        _indices.Add((uint)ibr);
        _indices.Add((uint)ibl);
        _indices.Add((uint)itl);

        return this;
    }

    public SimpleModelBuilder AddCube(Vector3 min, Vector3 max)
    {
        Vector3 minBotLeft = min;                               // 1
        Vector3 minTopLeft = new Vector3(min.X, min.Y, max.Z);  // 2
        Vector3 minTopRight = new Vector3(max.X, min.Y, max.Z); // 3
        Vector3 minBotRight = new Vector3(max.X, min.Y, min.Z); // 4

        Vector3 maxBotLeft = new Vector3(min.X, max.Y, min.Z);  // 5
        Vector3 maxTopLeft = new Vector3(min.X, max.Y, max.Z);  // 6
        Vector3 maxTopRight = max;                              // 7
        Vector3 maxBotRight = new Vector3(max.X, max.Y, min.Z); // 8

        AddQuad(minBotLeft, minBotRight, minTopRight, minTopLeft); // Bottom Quad
        AddQuad(maxTopLeft, maxBotLeft, minBotLeft, minTopLeft); // Left Quad
        AddQuad(maxTopRight, maxTopLeft, minTopLeft, minTopRight); // Front Quad
        AddQuad(maxBotRight, maxTopRight, minTopRight, minBotRight); // Right Quad
        AddQuad(maxBotLeft, maxTopRight, minBotRight, minBotLeft); // Back Quad
        AddQuad(maxTopLeft, maxTopRight, maxBotRight, maxBotLeft); // Top Quad
        
        return this;
    }
}