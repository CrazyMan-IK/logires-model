using BrailleCanvas.Interfaces;

namespace BrailleCanvas.Models;

public struct BBox
{
    public BBox()
    {
        
    }

    public Vector2 Min { get; set; } = new Vector2(float.MaxValue, float.MaxValue);
    public Vector2 Max { get; set; } = new Vector2(float.MinValue, float.MinValue);

    public override string ToString()
    {
        return $"Min: {{{Min}}}, Max: {{{Max}}}";
    }
}