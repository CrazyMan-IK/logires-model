using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class ColorExtensions
{
    public static Color Lerp(Color a, Color b, float t)
    {
        return a + (b - a) * t;
    }
    
    public static Color Lerp(float t, params Color[] colors)
    {
        var delta = t * (colors.Length + 1.0f);

        var res = colors.Select((x, i) => (index: i * 1.0f, color: x)).Aggregate(new Color(), (res, x) =>
        {
            if (x.index == 0)
            {
                return x.color;
            }

            return Lerp(res, x.color, MathExtensions.Clamp(delta - x.index, 0, 1));
        });

        return res;
    }

    public static Color MixColors(IEnumerable<Color> colors)
    {
        var acc = new Color();

        int count = 0;
        foreach (var color in colors)
        {
            acc += color.AsRYB();
            count++;
        }

        acc /= count;

        var result = acc.AsRGB();

        return result;
    }
}
