using System.Text;

namespace SteveClient.Engine.Rendering.Shaders.Processor;

public static class ShaderProcessor
{
    private static readonly Dictionary<string, ShaderLibrary> ShaderLibraries = new();

    public static string Process(string shaderSource)
    {
        StringBuilder source = new StringBuilder();
        
        foreach (var line in shaderSource.Split('\r', '\n'))
        {
            if (line.StartsWith("#include"))
            {
                string libPath = line.Split(' ').Last();
                ShaderLibrary lib = GetShaderLibrary(libPath);

                source.AppendLine(lib.Source);
                
                continue;
            }

            source.AppendLine(line);
        }

        return source.ToString();
    }

    private static ShaderLibrary GetShaderLibrary(string path)
    {
        if (!ShaderLibraries.ContainsKey(path))
            ShaderLibraries[path] = new ShaderLibrary(path);

        return ShaderLibraries[path];
    }
}