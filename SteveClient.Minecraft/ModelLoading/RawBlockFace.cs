using OpenTK.Mathematics;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.ModelLoading;

public class RawBlockFace
{
    public string Texture;
    public Vector3 TopLeft;
    public Vector3 TopRight;
    public Vector3 BottomLeft;
    public Vector3 BottomRight;
    public Vector2 Xy;
    public Vector2 Uy;
    public Vector2 Xv;
    public Vector2 Uv;
    public Directions CullFace;
}