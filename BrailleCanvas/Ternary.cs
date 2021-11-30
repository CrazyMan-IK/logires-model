namespace BrailleCanvas;

public class Ternary<T>
{
    private readonly Func<bool> _getter;
    private readonly T _left;
    private readonly T _right;

    public Ternary(Func<bool> getter, T left, T right)
    {
        _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        _left = left;
        _right = right;
    }

    /*public static implicit operator T(Ternary<T> ternary)
    {
        return ternary._getter() ? ternary._left : ternary._right;
    }*/

    public T Value => _getter() ? _left : _right;
}
