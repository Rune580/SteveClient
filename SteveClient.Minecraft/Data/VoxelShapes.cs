using OpenTK.Mathematics;
using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Data;

public static class VoxelShapes
{
    private static readonly List<VoxelShape> VoxelShapeList = new();

    //public static int PlayerShapeIndex = Add(new VoxelShape(new Aabb(new Vector3(-0.3f, 0f, -0.3f), new Vector3(0.3f, 1.8f, 0.3f))));

    public static int Add(VoxelShape voxelShape)
    {
        int index = VoxelShapeList.IndexOf(voxelShape);

        if (index >= 0)
            return index;

        index = VoxelShapeList.Count;

        VoxelShapeList.Add(voxelShape);

        return index;
    }

    public static int Add(string voxelShapeData)
    {
        VoxelShape voxelShape = new VoxelShape(voxelShapeData);

        return Add(voxelShape);
    }

    public static VoxelShape Get(int index)
    {
        return VoxelShapeList[index];
    }
}