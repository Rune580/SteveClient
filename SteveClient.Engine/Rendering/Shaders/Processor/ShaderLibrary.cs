using OpenTK.Graphics.OpenGL4;

namespace SteveClient.Engine.Rendering.Shaders.Processor;

public readonly struct ShaderLibrary
{
    public readonly string Source;
    
    public ShaderLibrary(string path)
    {
        Source = Assets.ReadEmbeddedShader(path);
    }
}