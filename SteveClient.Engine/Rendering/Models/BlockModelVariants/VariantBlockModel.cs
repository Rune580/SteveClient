namespace SteveClient.Engine.Rendering.Models.BlockModelVariants;

public class VariantBlockModel : IBlockModel
{
    private readonly Entry[] _variants;
    private int _count;
    private int _totalWeight;

    public VariantBlockModel(int capacity)
    {
        _variants = new Entry[capacity];
        _count = 0;
        _totalWeight = 0;
    }

    public void Add(BlockModel model, int weight)
    {
        if (_count >= _variants.Length)
            throw new IndexOutOfRangeException();
        
        _variants[_count] = new Entry(model, weight);
        _count++;

        _totalWeight += weight;
    }
    
    public BlockModel Get()
    {
        return _variants[0].Model;
    }

    public BlockModel Get(Random random)
    {
        int num = random.Next(0, _totalWeight);

        for (int i = 0; i < _variants.Length; i++)
        {
            int chance = _variants[i].Weight + i;

            if (num < chance)
                return _variants[i].Model;
        }

        throw new Exception();
    }
    
    private readonly struct Entry
    {
        public readonly BlockModel Model;
        public readonly int Weight;

        public Entry(BlockModel model, int weight)
        {
            Model = model;
            Weight = weight;
        }
    }
}