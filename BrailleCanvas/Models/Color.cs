using BrailleCanvas.Extensions;

namespace BrailleCanvas.Models;

public struct Color
{
    private static Dictionary<int, Color> _cachedRYB = new Dictionary<int, Color>();
    private static Dictionary<int, Color> _cachedRGB = new Dictionary<int, Color>();

    public Color() : this(0, 0, 0)
    {

    }

    public Color(float r, float g, float b)
    {
        R = r;
        G = g;
        B = b;
    }

    public float R { get; private set; } = 0;
    public float G { get; private set; } = 0;
    public float B { get; private set; } = 0;

    public string AsEscapeSequence()
    {
        return $"\x1b[38;2;{(int)MathExtensions.Round(R)};{(int)MathExtensions.Round(G)};{(int)MathExtensions.Round(B)}m";
    }

    public override string ToString()
    {
        return $"R: {R}, G: {G}, B: {B}";
    }

    public static Color MixColors(IEnumerable<Color> colors)
    {
        var acc = new Color();

        //Console.WriteLine("|------------|");

        int count = 0;
        foreach (var color in colors)
        {
            //Console.WriteLine(color);
            //Console.WriteLine(color.AsRYB());
            acc += color.AsRYB();
            count++;
        }

        //Console.WriteLine();
        //Console.WriteLine(acc);

        acc /= count;

        var result = acc.AsRGB();

        return result;
    }

    public static Color operator +(Color a, Color b)
    {
        return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
    }

    public static Color operator +(Color a, float b)
    {
        return new Color(a.R + b, a.G + b, a.B + b);
    }

    public static Color operator -(Color a, float b)
    {
        return new Color(a.R - b, a.G - b, a.B - b);
    }

    public static Color operator *(Color a, float b)
    {
        return new Color(a.R * b, a.G * b, a.B * b);
    }

    public static Color operator /(Color a, float b)
    {
        return new Color(a.R / b, a.G / b, a.B / b);
    }

    private Color AsRYB()
    {
        var hashCode = GetHashCode();
        if (_cachedRYB.TryGetValue(hashCode, out var value))
        {
            return value;
        }

        var newColor = new Color(R, G, B);

        var w = MathF.Min(MathF.Min(newColor.R, newColor.G), newColor.B);
        newColor.R -= w;
        newColor.G -= w;
        newColor.B -= w;

        var mg = MathF.Max(MathF.Max(newColor.R, newColor.G), newColor.B);

        var y = Math.Min(newColor.R, newColor.G);
        newColor.R -= y;
        newColor.G -= y;

        if (newColor.G != 0 && newColor.B != 0)
        {
            newColor.G /= 2;
            newColor.B /= 2;
        }

        y += newColor.G;
        newColor.B += newColor.G;

        newColor.G = y;

        var my = MathF.Max(MathF.Max(newColor.R, newColor.G), newColor.B);
        if (my != 0)
        {
            var n = mg / my;
            newColor.R *= n;
            newColor.G *= n;
            newColor.B *= n;
        }

        newColor.R += w;
        newColor.G += w;
        newColor.B += w;

        _cachedRYB[hashCode] = newColor;
        return newColor;
    }

    private Color AsRGB()
    {
        var hashCode = GetHashCode();
        if (_cachedRGB.TryGetValue(hashCode, out var value))
        {
            return value;
        }

        var newColor = new Color(R, G, B);

        var w = MathF.Min(MathF.Min(newColor.R, newColor.G), newColor.B);
        newColor.R -= w;
        newColor.G -= w;
        newColor.B -= w;

        var my = MathF.Max(MathF.Max(newColor.R, newColor.G), newColor.B);

        var g = Math.Min(newColor.G, newColor.B);
        newColor.G -= g;
        newColor.B -= g;

        if (newColor.G != 0 && newColor.B != 0)
        {
            newColor.G *= 2;
            newColor.B *= 2;
        }

        newColor.R += newColor.G;
        g += newColor.G;

        newColor.G = g;

        var mg = MathF.Max(MathF.Max(newColor.R, newColor.G), newColor.B);
        if (mg != 0)
        {
            var n = my / mg;
            newColor.R *= n;
            newColor.G *= n;
            newColor.B *= n;
        }

        newColor.R += w;
        newColor.G += w;
        newColor.B += w;

        _cachedRGB[hashCode] = newColor;
        return newColor;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(R, G, B);
    }
}
