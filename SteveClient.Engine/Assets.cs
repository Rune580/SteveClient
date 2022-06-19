using System.Reflection;

namespace SteveClient.Engine;

public static class Assets
{
    private static Stream GetEmbeddedAssetStream(string name)
    {
        Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

        if (stream is null)
            throw new Exception($"Could not load embedded asset {name}!");

        return stream;
    }
        
    public static string ReadEmbeddedShader(string name)
    {
        using StreamReader reader = new StreamReader(GetEmbeddedAssetStream(name));
            
        var data = reader.ReadToEnd();
            
        reader.Dispose();

        return data;
    }
}