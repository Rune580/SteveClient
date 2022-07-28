namespace SteveClient.Engine.Rendering.Models.BlockModelVariants;

public interface IBlockModel
{
    public BlockModel Get();

    public BlockModel Get(Random random);
}