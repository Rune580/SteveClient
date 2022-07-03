namespace SteveClient.Minecraft.ModelLoading;

public class RawBlockModel
{
    public readonly string ResourceName;
    public readonly RawBlockFace[] Faces;

    public RawBlockModel(string resourceName, RawBlockFace[] faces)
    {
        ResourceName = resourceName;
        Faces = faces;
    }
}