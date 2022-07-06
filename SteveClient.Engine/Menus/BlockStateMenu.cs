using System.Globalization;
using System.Text;
using ImGuiNET;
using SteveClient.Engine.Engines.Tools;

namespace SteveClient.Engine.Menus;

public class BlockStateMenu : IMenu
{
    private readonly byte[] _resourceNameBuffer = new byte[100];
    private int _textLength;
    
    public void Draw()
    {
        ImGui.Begin("BlockState");

        ResourceNameInput();

        if (ImGui.Button("Load"))
        {
            string resourceName = GetResourceNameInput();

            if (int.TryParse(resourceName, out int blockStateId))
            {
                SpawnBlockModelEntityEngine.LoadBlockState(blockStateId);
            }
            else
            {
                SpawnBlockModelEntityEngine.LoadBlockState(resourceName);
            }
        }

        ImGui.End();
    }

    private unsafe void ResourceNameInput()
    {
        byte[] tempBuf = new byte[100];
        
        Array.Copy(_resourceNameBuffer, tempBuf, _resourceNameBuffer.Length);
        
        ImGui.InputText("ResourceName", tempBuf, 100, ImGuiInputTextFlags.CallbackAlways, Callback);
        
        Array.Clear(_resourceNameBuffer);
        Array.Copy(tempBuf, _resourceNameBuffer, _textLength);
    }

    private unsafe int Callback(ImGuiInputTextCallbackData* data)
    {
        _textLength = data->BufTextLen;
        
        return 0;
    }

    private string GetResourceNameInput()
    {
        return Encoding.UTF8.GetString(_resourceNameBuffer).Replace("\0", "");
    }
}