﻿using System.Collections;

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

    public static implicit operator FontString(string text) => new(text);

    public new string ToString() => RawString;

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
    
    #region Enumeration

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

    #endregion
}