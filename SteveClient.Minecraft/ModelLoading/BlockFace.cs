using OpenTK.Mathematics;

namespace SteveClient.Minecraft.ModelLoading;

public class BlockFace
{
    public string Texture;
    public Vector3 TopLeft;
    public Vector3 TopRight;
    public Vector3 BottomLeft;
    public Vector3 BottomRight;
    public Vector2 UvMin;
    public Vector2 UvMax;
}