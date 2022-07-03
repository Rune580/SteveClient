using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.UnBaked;

public class TexturedQuad
{
    public readonly Vector3[] Vertices;
    public readonly Vector2[] Uvs;
    public readonly uint[] Triangles;
    public readonly string TextureResourceName;

    public TexturedQuad(Vector3[] vertices, Vector2[] uvs, uint[] triangles, string textureResourceName)
    {
        Vertices = vertices;
        Uvs = uvs;
        Triangles = triangles;
        TextureResourceName = textureResourceName;
    }

    public TexturedQuad(Vector3[] vertices, Vector2[] uvs, string textureResourceName)
    {
        Vertices = vertices;
        Uvs = uvs;
        TextureResourceName = textureResourceName;

        Triangles = new uint[] { 0, 2, 1, 3, 1, 2 };
    }
}