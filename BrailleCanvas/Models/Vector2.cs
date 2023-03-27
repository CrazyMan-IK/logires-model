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

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }

    public static Vector2 Zero => new Vector2(0, 0);
    public static Vector2 One => new Vector2(1, 1);
}

public class RefVector2 : IReadOnlyVector2<float>
{
    private Vector2 _vector;

    public RefVector2() : this(0, 0)
    {

    }

    public RefVector2(float x, float y)
    {
        _vector = new Vector2(x, y);
    }

    public float X
    {
        get => _vector.X;
        set
        {
            _vector.X = value;
        }
    }
    public float Y
    {
        get => _vector.Y;
        set
        {
            _vector.Y = value;
        }
    }

    public IReadOnlyVector2<float> Add(IReadOnlyVector2<float> b)
    {
        return _vector.Add(b);
    }
    public IReadOnlyVector2<float> Subtract(IReadOnlyVector2<float> b)
    {
        return _vector.Subtract(b);
    }
    public IReadOnlyVector2<float> Multiply(float b)
    {
        return _vector.Multiply(b);
    }
    public IReadOnlyVector2<float> Divide(float b)
    {
        return _vector.Divide(b);
    }

    public override int GetHashCode()
    {
        return _vector.GetHashCode();
    }

    public override string ToString()
    {
        return _vector.ToString();
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
    public IReadOnlyVector2<int> Multiply(int b)
    {
        return new Vector2Int(X * b, Y * b);
    }
    public IReadOnlyVector2<int> Divide(int b)
    {
        return new Vector2Int(X / b, Y / b);
    }

    public override string ToString()
    {
        return $"X: {X}, Y: {Y}";
    }
}
