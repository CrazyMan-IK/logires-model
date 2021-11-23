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
	
	public override string ToString()
	{
	  return $"X: {X}, Y: {Y}";
	}
}
