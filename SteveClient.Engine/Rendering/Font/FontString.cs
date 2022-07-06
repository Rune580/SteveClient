using System.Collections;
using OpenTK.Mathematics;

namespace SteveClient.Engine.Rendering.Font;

public struct FontString : IEquatable<FontString>, IEnumerable<Character>
{
    public TrueTypeFont Font;
    public string RawString;

    public FontString(TrueTypeFont font, string rawString)
    {
        Font = font;
        RawString = rawString;
    }

    public FontString(string text) : this(Fonts.Default, text) { }

    public Character this[int i] => Font[RawString[i]];

    public int Length => RawString.Length;

    // public Vector2i GetSize()
    // {
    //     int width = 0;
    //     int height = 0;
    //
    //     for (int i = 0; i < Length; i++)
    //     {
    //         this[i].
    //     }
    // }

    #region Equality Members

    public bool Equals(FontString other)
    {
        return Font.Equals(other.Font) && RawString == other.RawString;
    }

    public override bool Equals(object? obj)
    {
        return obj is FontString other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Font, RawString);
    }

    public static bool operator ==(FontString left, FontString right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FontString left, FontString right)
    {
        return !left.Equals(right);
    }

    #endregion

    public CharacterEnumerator GetEnumerator()
    {
        return new CharacterEnumerator(this);
    }
    
    IEnumerator<Character> IEnumerable<Character>.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}