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
}