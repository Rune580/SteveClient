using SteveClient.Engine.Rendering.Baked;
using SteveClient.Engine.Rendering.Models;

namespace SteveClient.Engine.Rendering.RenderLayers;

public interface IBakedRenderDataHandler : IVertexConsumer
{
    public void UploadRenderData(IBakedRenderData renderData);
}