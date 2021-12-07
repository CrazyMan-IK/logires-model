using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Extensions;

public static class MathExtensions
{
    public static float Mod(float a, float b)
    {
        return a - b * MathF.Floor(a / b);
    }

    public static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    public static float Round(float value)
    {
        return MathF.Round(value, MidpointRounding.AwayFromZero);
    }

    public static float Round(float value, int digits)
    {
        return MathF.Round(value, digits, MidpointRounding.AwayFromZero);
    }

    public static float Max(params float[] values)
    {
    	var max = float.MinValue;
    	
    	foreach (var value in values)
    	{
    		if (value > max)
    		{
    			max = value;
    		}
    	}

    	return max;
    }

    public static float Clamp(float value, float min, float max)
    {
        return MathF.Min(MathF.Max(value, min), max);
    }
}
