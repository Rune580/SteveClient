namespace SteveClient.Engine.Rendering.VertexData;

public interface IVertex
{
    int GetSize();
    
    // I wish there was a way to force an inheriting class to implement a constant, while also being able to call that from an interface.
    int GetStride();

    float[] VertexData { get; }
}

public static class VertexExtensions
{
    /// <returns>A Vertex array as floats.</returns>
    /// <exception cref="IndexOutOfRangeException">Thrown when used on an empty Vertex array.</exception>
    public static float[] VertexData(this IVertex[] vertices)
    {
        if (vertices.Length == 0)
            throw new IndexOutOfRangeException("Vertex array can't be empty!");

        int size = vertices[0].GetSize();

        float[] data = new float[size * vertices.Length];
        
        for (int i = 0; i < vertices.Length; i++)
        {
            int offset = size * i;
            
            Array.Copy(vertices[i].VertexData, 0, data, offset, size);
        }

        return data;
    }
}