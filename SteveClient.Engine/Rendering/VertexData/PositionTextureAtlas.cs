﻿using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct PositionTextureAtlas : IVertex
{
    public const int Size = 3 + 2 + 1;
    public const int Stride = sizeof(float) * Size;
    
    public int GetSize() => Size;
    public int GetStride() => Stride;
    
    public float[] VertexData { get; }

    public PositionTextureAtlas(Vector3 position, Vector2 texture, int atlas)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position)
            .WithVector2(texture)
            .WithFloat(atlas);
    }
}