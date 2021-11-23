namespace BrailleCanvas.Models;

public struct Color
{
	public float R { get; private set; } = 0;
	public float G { get; private set; } = 0;
	public float B { get; private set; } = 0;

	public Color() : this(0, 0, 0)
	{
		
	}

	public Color(float r, float g, float b)
	{
		R = r;
		G = g;
		B = b;
	}

	public string AsEscapeSequence()
    {
		return $"\x1b[38;2;{(int)MathF.Round(R)};{(int)MathF.Round(G)};{(int)MathF.Round(B)}m";
	}

	public static Color MixColors(IEnumerable<Color> colors)
	{
		var count = colors.Count();
		var acc = new Color();

		foreach (var color in colors)
        {
			acc += color.AsRYB();
        }

		acc /= count;

		var result = acc.AsRGB();

		return result;
	}

	public static Color operator+(Color a, Color b)
	{
		return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
	}

	public static Color operator+(Color a, float b)
	{
		return new Color(a.R + b, a.G + b, a.B + b);
	}

	public static Color operator-(Color a, float b)
	{
		return new Color(a.R - b, a.G - b, a.B - b);
	}
	
	public static Color operator*(Color a, float b)
	{
		return new Color(a.R * b, a.G * b, a.B * b);
	}
	
	public static Color operator/(Color a, float b)
	{
		return new Color(a.R / b, a.G / b, a.B / b);
	}

	private Color AsRYB()
    {
		var newColor = new Color(R, G, B);

		var w = MathF.Min(MathF.Min(newColor.R, newColor.G), newColor.B);
		newColor -= w;

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
			newColor *= n;
        }

		newColor += w;

		return newColor;
    }

	private Color AsRGB()
    {
		var newColor = new Color(R, G, B);

		var w = MathF.Min(MathF.Min(newColor.R, newColor.G), newColor.B);
		newColor -= w;

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
			newColor *= n;
        }

		newColor += w;

		return newColor;
    }
}
