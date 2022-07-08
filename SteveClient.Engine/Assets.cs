using System.Reflection;

namespace SteveClient.Engine;

public static class Assets
{
    private static Stream GetEmbeddedAssetStream(string path)
    {
        path = "SteveClient.Engine." + path.Replace("/", ".");
        
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

        if (stream is null)
            throw new Exception($"Could not load embedded asset {path}!");

        return stream;
    }
        
    public static string ReadEmbeddedShader(string name)
    {
        string path = $"Resources/Shaders/{name}";
        using StreamReader reader = new StreamReader(GetEmbeddedAssetStream(path));
            
        var data = reader.ReadToEnd();
            
        reader.Dispose();

        return data;
    }
}