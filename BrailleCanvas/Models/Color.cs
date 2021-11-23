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

	public static Color operator+(Color a, Color b)
	{
		return new Color(a.R + b.R, a.G + b.G, a.B + b.B);
	}
	
	public static Color operator/(Color a, float b)
	{
		return new Color(a.R / b, a.G / b, a.B / b);
	}
}
