using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public static class VertexFactories
{
    private static readonly Dictionary<Type, VertexFactory> VertexFactoryMap = new();

    public static VertexFactory GetFactory(Type type) => VertexFactoryMap[type];

    #region Factories

    public static readonly VertexFactory PositionColorFactory = new(typeof(PositionColor),
        (positions, _, colors, _) =>
        {
            if (positions is null)
                throw new NullReferenceException();

            if (colors is null)
                throw new NullReferenceException();
            
            IVertex[] result = new IVertex[positions.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = new PositionColor(positions[i], colors[i]);

            return result;
        });

    public static readonly VertexFactory PositionTextureColorFactory = new(typeof(PositionTextureColor),
        (positions, _, colors, uvs) =>
        {
            if (positions is null)
                throw new NullReferenceException();

            if (colors is null)
                throw new NullReferenceException();

            if (uvs is null)
                throw new NullReferenceException();

            IVertex[] result = new IVertex[positions.Length];

            for (int i = 0; i < result.Length; i++)
                result[i] = new PositionTextureColor(positions[i], uvs[i], colors[i]);

            return result;
        });

    #endregion
    
    public class VertexFactory
    {
        public readonly Type HandledType;

        private readonly Func<Vector3[]?, Vector3[]?, Color4[]?, Vector2[]?, IVertex[]> _factoryFunc; // Position, Normals, Colors, Uvs

        public VertexFactory(Type handledType, Func<Vector3[]?, Vector3[]?, Color4[]?, Vector2[]?, IVertex[]> factoryFunc)
        {
            HandledType = handledType;
            _factoryFunc = factoryFunc;

            VertexFactoryMap[handledType] = this;
        }

        public IVertex[] Consume(Vector3[]? vertices, Vector3[]? normals, Color4[]? colors, Vector2[]? uvs)
        {
            return _factoryFunc.Invoke(vertices, normals, colors, uvs);
        }
    }
}