namespace SteveClient.Engine.Rendering.Textures.Atlas;

public class AtlasTexture : IAtlasTexture
{
    private readonly int _atlasLayer;
    
    public AtlasTexture(int atlasLayer)
    {
        _atlasLayer = atlasLayer;
    }

    public int GetAtlasLayer() => _atlasLayer;
}