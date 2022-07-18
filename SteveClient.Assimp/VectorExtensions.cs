using OpenTK.Mathematics;

namespace SteveClient.Assimp;

internal static class VectorExtensions
{
    internal static Vector3 AsOpenTkVector(this System.Numerics.Vector3 v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }

    internal static Vector2 AsOpenTkVector(this System.Numerics.Vector2 v)
    {
        return new Vector2(v.X, v.Y);
    }
}