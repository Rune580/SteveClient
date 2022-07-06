using ImGuiNET;
using SteveClient.Engine.Rendering.Definitions;

namespace SteveClient.Engine.Menus;

public class WireframeWidget : IMenu
{
    private bool[] _wireframes = Array.Empty<bool>();
    
    public void Draw()
    {
        ValidateArrays();
        
        ImGui.Begin("RenderLayers");
        
        ImGui.Columns(2);
        ImGui.Text("Wireframe");
        for (var i = 0; i < RenderLayerDefinitions.Instances.Count; i++)
        {
            ImGui.PushID($"wireframe_checkbox_{i}");
            ImGui.Checkbox("", ref _wireframes[i]);
            ImGui.PopID();

            RenderLayerDefinitions.Instances[i].Wireframe = _wireframes[i];
        }

        ImGui.NextColumn();
        
        ImGui.Text("Layers");
        for (var i = 0; i < RenderLayerDefinitions.Instances.Count; i++)
        {
            ImGui.LabelText($"{i}", "");
        }
        
        ImGui.End();
    }

    private void ValidateArrays()
    {
        if (_wireframes.Length != RenderLayerDefinitions.Instances.Count)
            _wireframes = new bool[RenderLayerDefinitions.Instances.Count];
    }
}