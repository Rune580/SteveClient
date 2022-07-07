using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Definitions;
using SteveClient.Engine.Rendering.VertexData;

namespace SteveClient.Engine.Rendering.RenderLayers;

public class ScreenSpaceRenderLayer<TVertex> : DefaultRenderLayer<TVertex> where TVertex : IVertex
{
    public ScreenSpaceRenderLayer(VertexDefinitions.VertexDefinition<TVertex> vertexDefinition) : base(vertexDefinition) { }

    public override void Render()
    {
        int offset = 0;
        
        foreach (var bakedModel in RenderData)
        {
            if (bakedModel.HasTexture)
                bakedModel.UseTexture();

            if (bakedModel.HasShaderProperties)
                bakedModel.ApplyShaderProperties(Shader);
            
            Shader.SetMatrix4("model", bakedModel.Transform);
            Shader.SetMatrix4("view", CameraState.ScreenSpaceViewMatrix);
            Shader.SetMatrix4("projection", CameraState.ScreenSpaceProjectionMatrix);
            
            Shader.SetColor("color", Color4.White);
            
            GL.DrawElements(Definition.PrimitiveType, bakedModel.Indices.Length, DrawElementsType.UnsignedInt, offset);

            offset += bakedModel.SizeOfIndices;
        }
    }
}