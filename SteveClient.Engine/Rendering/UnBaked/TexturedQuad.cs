using OpenTK.Mathematics;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Engine.Rendering.UnBaked;

public class TexturedQuad
{
    public readonly Vector3[] Vertices;
    public readonly Vector2[] Uvs;
    public readonly uint[] Triangles;
    public readonly string TextureResourceName;
    public Directions CullFace;

    public TexturedQuad(Vector3[] vertices, Vector2[] uvs, uint[] triangles, string textureResourceName, Directions cullFace = Directions.None)
    {
        Vertices = vertices;
        Uvs = uvs;
        Triangles = triangles;
        TextureResourceName = textureResourceName;
        CullFace = cullFace;
    }

    public TexturedQuad(Vector3[] vertices, Vector2[] uvs, string textureResourceName, Directions cullFace = Directions.None)
    {
        Vertices = vertices;
        Uvs = uvs;
        TextureResourceName = textureResourceName;
        CullFace = cullFace;

        Triangles = new uint[] { 0, 2, 1, 3, 1, 2 };
    }
}