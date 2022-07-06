using SteveClient.Engine.Rendering.VertexData;
using static SteveClient.Engine.Rendering.VertexData.VertexFactories;

namespace SteveClient.Engine.Rendering.RenderLayers;

public interface IVertexConsumer
{
    public VertexFactory GetVertexFactory();
}