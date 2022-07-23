using SteveClient.Minecraft.BlockStructs;

namespace SteveClient.Minecraft.Data.Schema.BlockStates;

public class VariantModels
{
    private readonly Dictionary<int, VariantModelJson[]> _variants = new();
    private readonly Dictionary<BlockProperties, int> _propertyVariantMap = new();

    private int _defaultIndex = -1;

    public VariantModelJson[] this[BlockProperties key] => Get(key);

    public int Count => _variants.Count;

    internal void Add(BlockProperties key, VariantModelJson[] variants)
    {
        int index = _variants.Count;

        _propertyVariantMap[key] = index;
        _variants[index] = variants;

        if (key.Properties.Length == 0)
            _defaultIndex = index;
    }

    public VariantModelJson[] Get(BlockProperties key)
    {
        foreach (var (blockProps, index) in _propertyVariantMap)
        {
            if (key == blockProps)
                return _variants[index];


            bool match = true;
            foreach (var curProp in blockProps.Properties)
            {
                if (key.TryGet(curProp.Property, out var keyProp))
                {
                    match = curProp == keyProp;
                }
                else
                {
                    match = false;
                }

                if (!match)
                {
                    break;
                }
            }

            if (match)
                return _variants[index];
        }

        if (_defaultIndex > -1)
            return _variants[_defaultIndex];

        throw new KeyNotFoundException();
    }
}