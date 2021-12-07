using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas;

public class Gradient : IHasValue<Color>
{
    private readonly Func<float> _getter;
    private readonly Color[] _colors;

    public Gradient(Func<float> getter, params Color[] colors)
    {
        _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        _colors = colors;
    }

    /*public static implicit operator T(Ternary<T> ternary)
    {
        return ternary._getter() ? ternary._left : ternary._right;
    }*/

    public Color Value => ColorExtensions.Lerp(_getter(), _colors);
}
