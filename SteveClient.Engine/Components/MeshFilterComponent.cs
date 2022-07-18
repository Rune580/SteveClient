using SteveClient.Engine.Rendering.Models;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct MeshFilterComponent : IEntityComponent
{
    public int MeshIndex;

    public MeshFilterComponent(int meshIndex)
    {
        MeshIndex = meshIndex;
    }

    public MeshFilterComponent(InternalMesh internalMesh) : this(internalMesh.Index) { }
}