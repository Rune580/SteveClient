using SteveClient.Minecraft.Numerics;

namespace SteveClient.Minecraft.Data.Structs;

public abstract class BlockProperty
{ 
    public string Property { get; }
    public object BoxedValue { get; }
    
    public BlockProperty(string property, object boxedValue)
    {
        Property = property;
        BoxedValue = boxedValue;
    }

    public bool Equals(BlockProperty other)
    {
        if (Property != other.Property)
            return false;

        if (this is BoolBlockProperty boolProp && other is BoolBlockProperty otherBoolProp)
            return boolProp.Value == otherBoolProp.Value;

        if (this is IntBlockProperty intProp && other is IntBlockProperty otherIntProp)
            return intProp.Value == otherIntProp.Value;

        if (this is DirectionBlockProperty dirProp && other is DirectionBlockProperty otherDirProp)
            return dirProp.Value == otherDirProp.Value;

        if (this is StringBlockProperty strProp && other is StringBlockProperty otherStrProp)
            return string.Equals(strProp.Value, otherStrProp.Value, StringComparison.CurrentCultureIgnoreCase);

        return BoxedValue == other.BoxedValue;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        
        if (ReferenceEquals(this, obj))
            return true;
        
        return Equals((BlockProperty)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Property, BoxedValue);
    }

    public static bool operator ==(BlockProperty left, BlockProperty right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BlockProperty left, BlockProperty right)
    {
        return !(left == right);
    }

    public static BlockProperty Parse(string name, string rawValue)
    {
        if (rawValue is "true" or "false")
            return new BoolBlockProperty(name, rawValue is "true");
        
        if (rawValue.Contains('"'))
            rawValue = rawValue.Replace("\"", "");

        if (int.TryParse(rawValue, out int resultInt))
            return new IntBlockProperty(name, resultInt);
        
        if (Enum.TryParse(rawValue, true, out Directions resultDir))
            return new DirectionBlockProperty(name, resultDir);

        return new StringBlockProperty(name, rawValue);
    }
}

public class StringBlockProperty : BlockProperty
{
    public string Value { get; }
    
    public StringBlockProperty(string property, string value) : base(property, value)
    {
        Value = value;
    }
}

public class BoolBlockProperty : BlockProperty
{
    public bool Value { get; }

    public BoolBlockProperty(string property, bool value) : base(property, value)
    {
        Value = value;
    }
}

public class DirectionBlockProperty : BlockProperty
{
    public Directions Value { get; }

    public DirectionBlockProperty(string property, Directions value) : base(property, value)
    {
        Value = value;
    }
}

public class IntBlockProperty : BlockProperty
{
    public int Value { get; }

    public IntBlockProperty(string property, int value) : base(property, value)
    {
        Value = value;
    }
}