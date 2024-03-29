﻿using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct BlockVertex : IVertex
{
    public const int Size = 3 + 3 + 3 + 2 + 1 + 1;
    public const int Stride = sizeof(float) * Size;

    public int GetSize() => Size;
    public int GetStride() => Stride;
    
    public float[] VertexData { get; }

    public BlockVertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector2 texture, int atlas, int blockPos)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position)
            .WithVector3(normal)
            .WithVector3(tangent)
            .WithVector2(texture)
            .WithFloat(atlas)
            .WithFloat(blockPos);
    }
}