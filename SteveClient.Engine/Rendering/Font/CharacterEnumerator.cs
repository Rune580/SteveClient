using System.Collections;

namespace SteveClient.Engine.Rendering.Font;

public sealed class CharacterEnumerator : IEnumerator<Character>
{
    private FontString _fontString;
    private int _index;
    
    public CharacterEnumerator(FontString fontString)
    {
        _fontString = fontString;
        _index = -1;
    }
    
    public bool MoveNext()
    {
        if (_index >= _fontString.RawString.Length - 1)
            return false;

        _index++;

        return true;
    }

    public void Reset()
    {
        _index = -1;
    }

    public Character Current => _fontString[_index];

    object IEnumerator.Current => Current;

    public void Dispose() { }
}