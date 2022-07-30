using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Numerics;

public struct Aabb
{
    public static readonly Aabb Block = new(Vector3.Zero, Vector3d.One);
    public static readonly Aabb Empty = new(Vector3d.Zero, Vector3d.Zero);
    
    public Vector3d Min { get; private set; }
    public Vector3d Max { get; private set; }
    
    public Aabb(Vector3d min, Vector3d max)
    {
        Min = min;
        Max = max;
    }
    
    public Aabb(Vector3 min, Vector3 max) : this((Vector3d)min, (Vector3d)max) { }
    
    public Aabb(Aabb other) : this(other.Min, other.Max) { }

    public Vector3d Center => (Min + Max) / 2d;
    
    public List<Vector3i> GetBlockPositions()
    {
        Vector3i min = Min.AsBlockPos();
        Vector3i max = Max.AsBlockPos();
            
        List<Vector3i> blocks = new List<Vector3i>();
            
        for (int x = min.X; x <= max.X; x++)
        {
            for (int y = min.Y; y <= max.Y; y++)
            {
                for (int z = min.Z; z <= max.Z; z++)
                {
                    blocks.Add(new Vector3i(x, y, z));
                }
            }
        }

        return blocks;
    }

    public bool Intersects(Aabb other)
    {
        return Min.X < other.Max.X && Max.X > other.Min.X &&
               Min.Y < other.Max.Y && Max.Y > other.Min.Y &&
               Min.Z < other.Max.Z && Max.Z > other.Min.Z;
    }

    public bool Contains(Vector3d pos)
    {
        return Min.X <= pos.X && Max.X >= pos.X &&
               Min.Y <= pos.Y && Max.Y >= pos.Y &&
               Min.Z <= pos.Z && Max.Z >= pos.Z;
    }

    public Aabb Face(Vector3d dir)
    {
        var facing = GetAxis(dir);

        return facing switch
        {
            Directions.Down => new Aabb(Min, new Vector3d(Max.X, Min.Y, Max.Z)),
            Directions.Up => new Aabb(new Vector3d(Min.X, Max.Y, Min.Z), Max),
            Directions.North => new Aabb(Min, new Vector3d(Max.X, Max.Y, Min.Z)),
            Directions.South => new Aabb(new Vector3d(Min.X, Min.Y, Max.Z), Max),
            Directions.West => new Aabb(Min, new Vector3d(Min.X, Max.Y, Max.Z)),
            Directions.East => new Aabb(new Vector3d(Max.X, Min.Y, Min.Z), Max),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Aabb Offset(Vector3d offset)
    {
        return new Aabb(Min + offset, Max + offset);
    }

    public Aabb Offset(Directions direction, float offset)
    {
        Vector3d axisOffset = direction.AsVector3() * offset;

        return new Aabb(Min + axisOffset, Max + axisOffset);
    }
    
    public Aabb Offset(Directions direction, double offset)
    {
        Vector3d axisOffset = direction.AsVector3();

        axisOffset *= offset;

        return new Aabb(Min + axisOffset, Max + axisOffset);
    }

    public Aabb Extend(Vector3d extend)
    {
        Vector3d min = new Vector3d(Min);
        Vector3d max = new Vector3d(Max);

        if (extend.X < 0)
        {
            min.X += extend.X;
        }
        else
        {
            max.X += extend.X;
        }

        if (extend.Y < 0)
        {
            min.Y += extend.Y;
        }
        else
        {
            max.Y += extend.Y;
        }
        
        if (extend.Z < 0)
        {
            min.Z += extend.Z;
        }
        else
        {
            max.Z += extend.Z;
        }

        return new Aabb(min, max);
    }

    public Aabb Extend(Vector3i extend)
    {
        return Extend((Vector3d)extend);
    }

    public Aabb Extend(Directions direction)
    {
        return Extend(direction.AsVector3());
    }

    public Aabb Grow(double size)
    {
        return new Aabb(Min.Sub(size), Max.Add(size));
    }

    public Aabb Grow(float size)
    {
        return new Aabb(Min.Sub(size), Max.Add(size));
    }

    public Aabb Shrink(double size)
    {
        return Grow(-size);
    }

    public Aabb Shrink(float size)
    {
        return Grow(-size);
    }

    public double ComputeOffset(Aabb other, double offset, Directions direction)
    {
        if (!Offset(direction, offset).Intersects(other))
            return offset;

        double thisMin = GetMin(direction);
        double thisMax = GetMax(direction);
        double otherMin = other.GetMin(direction);
        double otherMax = other.GetMax(direction);

        if (offset > 0 && thisMin <= otherMax + offset)
            return Math.Min(thisMin - otherMax, offset);

        if (offset < 0 && thisMax >= otherMin + offset)
            return Math.Min(thisMax - otherMin, offset);

        return offset;
    }

    private double GetMin(Directions directions)
    {
        return directions switch
        {
            Directions.West or Directions.East => Min.X,
            Directions.Down or Directions.Up => Min.Y,
            Directions.North or Directions.South => Min.Z,
            _ => throw new ArgumentOutOfRangeException(nameof(directions), directions, null)
        };
    }

    private double GetMax(Directions directions)
    {
        return directions switch
        {
            Directions.West or Directions.East => Max.X,
            Directions.Down or Directions.Up => Max.Y,
            Directions.North or Directions.South => Max.Z,
            _ => throw new ArgumentOutOfRangeException(nameof(directions), directions, null)
        };
    }
    
    public override int GetHashCode()
    {
        return Min.GetHashCode() + Min.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj is Aabb other && this == other;
    }

    public static Aabb operator +(Aabb left, Vector3d other) => left.Offset(other);
    
    public static Aabb operator +(Aabb left, Vector3 other) => left.Offset(other);

    public static Aabb operator +(Aabb left, Aabb right)
    {
        Vector3d min = new Vector3d(Math.Min(left.Min.X, right.Min.X), Math.Min(left.Min.Y, right.Min.Y), Math.Min(left.Min.Z, right.Min.Z));
        Vector3d max = new Vector3d(Math.Max(left.Max.X, right.Max.X), Math.Max(left.Max.Y, right.Max.Y), Math.Max(left.Max.Z, right.Max.Z));

        return new Aabb(min, max);
    }

    public static bool operator ==(Aabb left, Aabb right)
    {
        return left.Min == right.Min && left.Max == right.Max;
    }

    public static bool operator !=(Aabb left, Aabb right)
    {
        return !(left == right);
    }

    private static Directions GetAxis(Vector3d direction)
    {
        var dir = direction.Normalized();

        if (dir.Z < 0)
            return Directions.North;
        if (dir.Z > 0)
            return Directions.South;

        if (dir.X < 0)
            return Directions.West;
        if (dir.X > 0)
            return Directions.East;

        if (dir.Y < 0)
            return Directions.Down;
        if (dir.Y > 0)
            return Directions.Up;

        return Directions.None;
    }
}