using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Numerics;

public static class VectorExtensions
{
    public static Vector3i AsBlockPos(this Vector3d pos)
    {
        int x = (int)(pos.X < 0 ? Math.Floor(pos.X) : Math.Ceiling(pos.X));
        int y = (int)(pos.Y < 0 ? Math.Floor(pos.Y) : Math.Ceiling(pos.Y));
        int z = (int)(pos.Z < 0 ? Math.Floor(pos.Z) : Math.Ceiling(pos.Z));

        return new Vector3i(x, y, z);
    }
    
    public static Vector3i AsBlockPos(this Vector3 pos)
    {
        int x = (int)(pos.X < 0 ? Math.Floor(pos.X) : Math.Ceiling(pos.X));
        int y = (int)(pos.Y < 0 ? Math.Floor(pos.Y) : Math.Ceiling(pos.Y));
        int z = (int)(pos.Z < 0 ? Math.Floor(pos.Z) : Math.Ceiling(pos.Z));

        return new Vector3i(x, y, z);
    }

    public static Vector3i Above(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X, blockPos.Y + 1, blockPos.Z);
    }
    
    public static Vector3i Below(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X, blockPos.Y - 1, blockPos.Z);
    }
    
    public static Vector3i North(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X, blockPos.Y, blockPos.Z - 1);
    }
    
    public static Vector3i South(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X, blockPos.Y, blockPos.Z + 1);
    }
    
    public static Vector3i East(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X + 1, blockPos.Y, blockPos.Z);
    }
    
    public static Vector3i West(this Vector3i blockPos)
    {
        return new Vector3i(blockPos.X - 1, blockPos.Y, blockPos.Z);
    }

    public static Vector3d Add(this Vector3d vector, double num)
    {
        return new Vector3d(vector.X + num, vector.Y + num, vector.Z + num);
    }
    
    public static Vector3d Add(this Vector3d vector, float num)
    {
        return new Vector3d(vector.X + num, vector.Y + num, vector.Z + num);
    }

    public static Vector3d Sub(this Vector3d vector, double num)
    {
        return new Vector3d(vector.X - num, vector.Y - num, vector.Z - num);
    }
    
    public static Vector3d Sub(this Vector3d vector, float num)
    {
        return new Vector3d(vector.X - num, vector.Y - num, vector.Z - num);
    }
    
    
    public static Vector3 Add(this Vector3 vector, float num)
    {
        return new Vector3(vector.X + num, vector.Y + num, vector.Z + num);
    }
    
    public static Vector3 Sub(this Vector3 vector, float num)
    {
        return new Vector3(vector.X - num, vector.Y - num, vector.Z - num);
    }

    public static Vector3i Abs(this Vector3i vector)
    {
        return new Vector3i(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
    }
}