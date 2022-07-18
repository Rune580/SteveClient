using OpenTK.Mathematics;
using SteveClient.Assimp;
using SteveClient.Engine.Rendering.Builders;

namespace SteveClient.Engine.Rendering.Models;

public static class Primitives
{
    public static readonly SimpleInternalModel Cube = new SimpleModelBuilder()
        .AddCube(new Vector3(-0.3f, 0f, -0.3f), new Vector3(0.3f, 2f, 0.3f)).Build();
}