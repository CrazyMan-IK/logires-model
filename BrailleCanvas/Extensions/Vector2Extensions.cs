using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class Vector2Extensions
{
    public static IReadOnlyVector2<float> Lerp(IReadOnlyVector2<float> a, IReadOnlyVector2<float> b, float t)
    {
        var result = new Vector2(a.X, a.Y);
        result.X = MathExtensions.Lerp(result.X, b.X, t);
        result.Y = MathExtensions.Lerp(result.Y, b.Y, t);

        return result;//a + (b - a) * t;
    }
}