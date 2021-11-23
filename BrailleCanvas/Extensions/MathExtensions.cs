using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class MathExtensions
{
    public static float Mod(float a, float b)
    {
        return a - b * MathF.Floor(a / b);
    }
}
