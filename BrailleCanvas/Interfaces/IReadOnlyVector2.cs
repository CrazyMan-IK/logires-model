namespace BrailleCanvas.Interfaces;

public interface IReadOnlyVector2<T>
{
	T X { get; }
	T Y { get; }

    IReadOnlyVector2<T> Add(IReadOnlyVector2<T> b);
    IReadOnlyVector2<T> Subtract(IReadOnlyVector2<T> b);
    IReadOnlyVector2<T> Multiply(float b);
    IReadOnlyVector2<T> Divide(float b);

    public static IReadOnlyVector2<T> operator +(IReadOnlyVector2<T> a, IReadOnlyVector2<T> b)
    {
        return a.Add(b);
    }
    public static IReadOnlyVector2<T> operator -(IReadOnlyVector2<T> a, IReadOnlyVector2<T> b)
    {
        return a.Subtract(b);
    }
    public static IReadOnlyVector2<T> operator *(IReadOnlyVector2<T> a, float b)
    {
        return a.Multiply(b);
    }
    public static IReadOnlyVector2<T> operator /(IReadOnlyVector2<T> a, float b)
    {
        return a.Divide(b);
    }
}
