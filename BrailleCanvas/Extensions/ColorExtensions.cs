using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class ColorExtensions
{
    public static Color Lerp(Color a, Color b, float t)
    {
        return a + (b - a) * t;
    }
}
