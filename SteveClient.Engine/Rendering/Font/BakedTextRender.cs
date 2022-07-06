using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Font;

public struct BakedTextRender
{
    public Character[] Characters;
    public Matrix4[] Transforms;
    public Color4[] Colors;

    public BakedTextRender(Character[] characters, Matrix4[] transforms, Color4[] colors)
    {
        Characters = characters;
        Transforms = transforms;
        Colors = colors;
    }
    
    public BakedTextRender(Character[] characters, Matrix4[] transforms, Color4 color)
    {
        Characters = characters;
        Transforms = transforms;

        Colors = new Color4[characters.Length];
        Array.Fill(Colors, color);
    }
    
    public BakedTextRender(Character[] characters, Matrix4[] transforms)
    {
        Characters = characters;
        Transforms = transforms;

        Colors = new Color4[characters.Length];
        Array.Fill(Colors, Color4.White);
    }
}