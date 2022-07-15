using System.Text;
using OpenTK.Mathematics;
using SteveClient.Engine.AssetManagement;
using SteveClient.Engine.Engines.Tools;
using SteveClient.Engine.Rendering.Ui.Elements;
using SteveClient.Minecraft.Blocks;
using SteveClient.Minecraft.Data;

namespace SteveClient.Engine.Rendering.Ui.Widgets;

public class BlockStateLoaderWidget : BaseElementContainer
{
    private InputField _inputField;
    private Button _loadButton;
    private Button _exportButton;
    
    protected override void RegisterElements(in List<BaseUiElement> uiElements)
    {
        _inputField = new InputField(new Box2(10, 30, 10 + 160, 30 + 24));

        float min = 30;
        _loadButton = new Button(new Box2(min - 30, 30 + 24, min + 30, 30 + 24 + 24), "Load");
        _loadButton.ButtonPressed += LoadBlockState;

        min += 30 + 50;
        _exportButton = new Button(new Box2(min - 50, 30 + 24, min + 50, 30 + 24 + 24), "Export");
        _exportButton.ButtonPressed += ExportBlockModel;
        
        uiElements.Add(_inputField);
        uiElements.Add(_loadButton);
        uiElements.Add(_exportButton);
    }

    private void LoadBlockState()
    {
        string resourceName = _inputField.Text;
        
        if (int.TryParse(resourceName, out int blockStateId))
        {
            SpawnBlockModelEntityEngine.LoadBlockState(blockStateId);
        }
        else
        {
            SpawnBlockModelEntityEngine.LoadBlockState(resourceName);
        }
    }

    private void ExportBlockModel()
    {
        string resourceName = _inputField.Text;

        var model = ModelRegistry.BlockModels[resourceName];

        StringBuilder obj = new StringBuilder();

        foreach (var quad in model.Quads)
        {
            obj = AppendVertex(obj, model.Vertices[quad.Vertices[0]]);
            obj = AppendVertex(obj, model.Vertices[quad.Vertices[1]]);
            obj = AppendVertex(obj, model.Vertices[quad.Vertices[2]]);
            obj = AppendVertex(obj, model.Vertices[quad.Vertices[3]]);
        }
        
        foreach (var quad in model.Quads)
        {
            obj = AppendUv(obj, quad.Uvs[0]);
            obj = AppendUv(obj, quad.Uvs[1]);
            obj = AppendUv(obj, quad.Uvs[2]);
            obj = AppendUv(obj, quad.Uvs[3]);
        }

        int offset = 1;

        foreach (var quad in model.Quads)
        {
            obj.AppendLine($"f {offset + 0}/{offset + 0} {offset + 2}/{offset + 2} {offset + 1}/{offset + 1}");
            obj.AppendLine($"f {offset + 3}/{offset + 3} {offset + 1}/{offset + 1} {offset + 2}/{offset + 2}");

            offset += 4;
        }

        File.WriteAllText($"{resourceName}.obj", obj.ToString());
    }

    private StringBuilder AppendVertex(StringBuilder obj, Vector3 vertex)
    {
        obj.AppendLine($"v {vertex.X:F3} {vertex.Y:F3} {vertex.Z:F3}");

        return obj;
    }
    
    private StringBuilder AppendUv(StringBuilder obj, Vector2 uv)
    {
        obj.AppendLine($"vt {uv.X:F3} {uv.Y:F3}");

        return obj;
    }
}