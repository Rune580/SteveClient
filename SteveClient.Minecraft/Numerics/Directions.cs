using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Numerics;

[Flags]
public enum Directions
{
    None = 0,
    Down = 1,
    Up = 2,
    North = 4,
    South = 8,
    West = 16,
    East = 32
}

public static class DirectionExtensions
{
    public static Directions RotateAroundY(this Directions dir, float degrees)
    {
        int rotations = (int)(degrees / 90f);

        Directions result = dir;

        for (int i = 0; i < rotations; i++)
        {
            result = result switch
            {
                Directions.None => Directions.None,
                Directions.Down => Directions.Down,
                Directions.Up => Directions.Up,
                Directions.North => Directions.East,
                Directions.South => Directions.West,
                Directions.West => Directions.North,
                Directions.East => Directions.South,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return result;
    }

    public static Directions RotateAroundX(this Directions dir, float degrees)
    {
        int rotations = (int)(degrees / 90f);

        Directions result = dir;

        for (int i = 0; i < rotations; i++)
        {
            result = result switch
            {
                Directions.None => Directions.None,
                Directions.Down => Directions.South,
                Directions.Up => Directions.North,
                Directions.North => Directions.Down,
                Directions.South => Directions.Up,
                Directions.West => Directions.West,
                Directions.East => Directions.East,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        return result;
    }

    public static Vector3 AsVector3(this Directions dir)
    {
        return dir switch
        {
            Directions.None => Vector3.Zero,
            Directions.Down => new Vector3(0, -1, 0),
            Directions.Up => new Vector3(0, 1, 0),
            Directions.North => new Vector3(0, 0, -1),
            Directions.South => new Vector3(0, 0, 1),
            Directions.West => new Vector3(-1, 0, 0),
            Directions.East => new Vector3(1, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
    
    public static Vector3i AsVector3i(this Directions dir)
    {
        return dir switch
        {
            Directions.None => Vector3i.Zero,
            Directions.Down => new Vector3i(0, -1, 0),
            Directions.Up => new Vector3i(0, 1, 0),
            Directions.North => new Vector3i(0, 0, -1),
            Directions.South => new Vector3i(0, 0, 1),
            Directions.West => new Vector3i(-1, 0, 0),
            Directions.East => new Vector3i(1, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}