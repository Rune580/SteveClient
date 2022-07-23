using SteveClient.Minecraft.Data.Structs;

namespace SteveClient.Minecraft.BlockStructs;

public class BlockProperties
{
    private readonly Dictionary<string, int> _namePropMap = new();
    internal readonly BlockProperty[] Properties;

    public BlockProperties()
    {
        Properties = Array.Empty<BlockProperty>();
    }

    public BlockProperties(params BlockProperty[] properties)
    {
        Properties = properties;
        
        IndexProperties();
    }

    public BlockProperties(string rawText)
    {
        if (string.IsNullOrEmpty(rawText))
        {
            Properties = Array.Empty<BlockProperty>();
            return;
        }
        
        string[] rawProps = rawText.Split(",");

        Properties = new BlockProperty[rawProps.Length];

        for (int i = 0; i < Properties.Length; i++)
        {
            string[] rawProp = rawProps[i].Split("=");

            if (rawProp.Length != 2)
                throw new Exception();
            
            Properties[i] = BlockProperty.Parse(rawProp[0], rawProp[1]);
        }

        IndexProperties();
    }

    private void IndexProperties()
    {
        for (int i = 0; i < Properties.Length; i++)
            _namePropMap[Properties[i].Property] = i;
    }

    public bool TryGet(string name, out BlockProperty property)
    {
        property = null!;
        
        if (!_namePropMap.TryGetValue(name, out int index))
            return false;

        property = Properties[index];

        return true;
    }

    public bool Equals(BlockProperties other)
    {
        if (Properties.Length != other.Properties.Length)
            return false;

        bool match = true;
        
        foreach (BlockProperty curProp in Properties)
        {
            bool curMatched = false;
            foreach (BlockProperty otherProp in other.Properties)
            {
                if (curProp == otherProp)
                    curMatched = true;
            }

            if (curMatched)
                continue;
            
            match = false;
            break;
        }

        return match;
    }  
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        
        if (ReferenceEquals(this, obj))
            return true;
        
        if (obj.GetType() != this.GetType())
            return false;
        
        return Equals((BlockProperties)obj);
    }

    public override int GetHashCode()
    {
        return Properties.GetHashCode();
    }

    public static bool operator ==(BlockProperties left, BlockProperties right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BlockProperties left, BlockProperties right)
    {
        return !(left == right);
    }
}