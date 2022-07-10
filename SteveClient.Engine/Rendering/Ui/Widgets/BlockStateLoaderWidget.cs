using OpenTK.Mathematics;
using SteveClient.Engine.Engines.Tools;
using SteveClient.Engine.Rendering.Ui.Elements;

namespace SteveClient.Engine.Rendering.Ui.Widgets;

public class BlockStateLoaderWidget : BaseElementContainer
{
    private InputField _inputField;
    private Button _button;
    
    protected override void RegisterElements(in List<BaseUiElement> uiElements)
    {
        _inputField = new InputField(new Box2(10, 30, 10 + 160, 30 + 24));
        float min = (10 + 160) / 2f;
        _button = new Button(new Box2(min - 30, 30 + 24, min + 30, 30 + 24 + 24), "Load");
        
        _button.ButtonPressed += ButtonPressed;
        
        uiElements.Add(_inputField);
        uiElements.Add(_button);
    }

    private void ButtonPressed()
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
}