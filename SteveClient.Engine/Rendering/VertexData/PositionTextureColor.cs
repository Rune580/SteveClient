﻿using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.VertexData;

public struct PositionTextureColor : IVertex
{
    public const int Size = 3 + 2 + 4;
    public const int Stride = sizeof(float) * Size;
    public int GetStride() => Stride;
    public int GetSize() => Size;
    public float[] VertexData { get; set; }

    public PositionTextureColor(Vector3 position, Vector2 texture, Color4 color)
    {
        VertexData = new VertexDataArray(Size)
            .WithVector3(position)
            .WithVector2(texture)
            .WithColor4(color);
    }
}