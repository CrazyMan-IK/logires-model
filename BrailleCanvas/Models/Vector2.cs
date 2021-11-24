using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;

namespace BrailleCanvas.Models;

public struct Vector2 : IReadOnlyVector2<float>
{
	public Vector2() : this(0, 0)
	{
		
	}

	public Vector2(float x, float y)
	{
		X = x;
		Y = y;
	}
	
	public float X { get; set; } = 0;
	public float Y { get; set; } = 0;

	public IReadOnlyVector2<float> Add(IReadOnlyVector2<float> b)
    {
		return new Vector2(X + b.X, Y + b.Y);
    }
	public IReadOnlyVector2<float> Subtract(IReadOnlyVector2<float> b)
    {
		return new Vector2(X - b.X, Y - b.Y);
    }
    public IReadOnlyVector2<float> Multiply(float b)
    {
		return new Vector2(X * b, Y * b);
    }
    public IReadOnlyVector2<float> Divide(float b)
    {
		return new Vector2(X / b, Y / b);
	}

	public override string ToString()
	{
		return $"X: {X}, Y: {Y}";
	}
}

public struct Vector2Int : IReadOnlyVector2<int>
{
	public Vector2Int() : this(0, 0)
	{
	
	}
	
	public Vector2Int(int x, int y)
	{
	  X = x;
	  Y = y;
	}
	
	public int X { get; set; } = 0;
	public int Y { get; set; } = 0;

	public IReadOnlyVector2<int> Add(IReadOnlyVector2<int> b)
	{
		return new Vector2Int(X + b.X, Y + b.Y);
	}
	public IReadOnlyVector2<int> Subtract(IReadOnlyVector2<int> b)
	{
		return new Vector2Int(X - b.X, Y - b.Y);
	}
	public IReadOnlyVector2<int> Multiply(float b)
	{
		return new Vector2Int((int)MathExtensions.Round(X * b), (int)MathExtensions.Round(Y * b));
	}
	public IReadOnlyVector2<int> Divide(float b)
	{
		return new Vector2Int((int)MathExtensions.Round(X / b), (int)MathExtensions.Round(Y / b));
	}

	public override string ToString()
	{
	  return $"X: {X}, Y: {Y}";
	}
}
