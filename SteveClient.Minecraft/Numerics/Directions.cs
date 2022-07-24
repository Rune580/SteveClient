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

        // for (int i = 0; i < rotations; i++)
        // {
        //     result = result switch
        //     {
        //         
        //     }
        // }

        return result;
    }
}