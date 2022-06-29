using System.Collections;
using OpenTK.Mathematics;

namespace SteveClient.Minecraft.Numerics;

public class VoxelShape : IEnumerable<Aabb>
{
    private readonly List<Aabb> _aabbs;

    public VoxelShape()
    {
        _aabbs = new List<Aabb>();
    }

    public VoxelShape(IEnumerable<Aabb> aabbs)
    {
        _aabbs = new List<Aabb>(aabbs);
    }

    public VoxelShape(string dataString) : this()
    {
        string[] data = dataString
            .Replace("AABB", "")
            .Replace("[", "")
            .Replace("]", "")
            .Replace("-> ", "")
            .Replace(",", "")
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (data.Length == 0)
            return;

        if (data.Length % 6 != 0)
            throw new InvalidOperationException();

        for (int i = 0; i < data.Length / 6; i++)
        {
            Vector3d min = new Vector3d(double.Parse(data[i]), double.Parse(data[i + 1]), double.Parse(data[i + 2]));
            Vector3d max = new Vector3d(double.Parse(data[i + 3]), double.Parse(data[i + 4]), double.Parse(data[i + 5]));
            
            Add(new Aabb(min, max));
        }
    }

    public bool Intersects(Aabb other)
    {
        return _aabbs.Any(aabb => aabb.Intersects(other));
    }

    public void Add(VoxelShape other)
    {
        foreach (var aabb in other._aabbs)
            _aabbs.Add(aabb);
    }
    
    public void Add(Aabb other)
    {
        _aabbs.Add(other);
    }

    public void Remove(VoxelShape other)
    {
        foreach (var aabb in other._aabbs)
            _aabbs.Remove(aabb);
    }

    public static VoxelShape operator +(VoxelShape left, Vector3 right)
    {
        List<Aabb> result = left._aabbs.Select(aabb => aabb + right).ToList();

        return new VoxelShape(result);
    }

    public static VoxelShape operator +(VoxelShape left, Aabb right)
    {
        VoxelShape result = new VoxelShape();
        
        result.Add(left);
        result.Add(right);

        return result;
    }

    public static VoxelShape operator +(VoxelShape left, VoxelShape right)
    {
        VoxelShape result = new VoxelShape();
        
        result.Add(left);
        result.Add(right);

        return result;
    }

    public static bool operator ==(VoxelShape left, VoxelShape right)
    {
        if (left._aabbs.Count != right._aabbs.Count)
            return false;

        for (int i = 0; i < left._aabbs.Count(); i++)
        {
            if (left._aabbs[i] != right._aabbs[i])
                return false;
        }

        return true;
    }

    public static bool operator !=(VoxelShape left, VoxelShape right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;

        VoxelShape other = (VoxelShape)obj;

        return this == other;
    }

    public override int GetHashCode()
    {
        return _aabbs.GetHashCode();
    }

    public IEnumerator<Aabb> GetEnumerator()
    {
        return _aabbs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}