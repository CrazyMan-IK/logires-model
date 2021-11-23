using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class Vector2Extensions
{
    public static IReadOnlyVector2<float> Lerp(IReadOnlyVector2<float> a, IReadOnlyVector2<float> b, float t)
    {
        return a + (b - a) * t;
    }
}
