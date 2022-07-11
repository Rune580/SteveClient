using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Shaders.Properties;

public class ColorShaderProperty : IShaderProperty
{
    private readonly string _name;
    private readonly Color4 _color;
    
    public ColorShaderProperty(string name, Color4 color)
    {
        _name = name;
        _color = color;
    }

    public void Apply(Shader shader)
    {
        shader.SetColor(_name, _color);
    }
}