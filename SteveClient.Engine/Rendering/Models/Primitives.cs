using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Builders;

namespace SteveClient.Engine.Rendering.Models;

public static class Primitives
{
    public static readonly SimpleModel Cube = new SimpleModelBuilder()
        .AddCube(new Vector3(-0.3f, 0f, -0.3f), new Vector3(0.3f, 2f, 0.3f)).Build();
    
    
}