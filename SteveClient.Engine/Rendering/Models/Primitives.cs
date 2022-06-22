using OpenTK.Mathematics;
using SteveClient.Engine.Rendering.Builders;

namespace SteveClient.Engine.Rendering.Models;

public static class Primitives
{
    public static readonly SimpleModel Cube = new SimpleModelBuilder()
        .AddCube(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, 0.5f)).Build();
    
    
}