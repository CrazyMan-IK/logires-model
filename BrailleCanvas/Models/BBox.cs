using BrailleCanvas.Interfaces;

namespace BrailleCanvas.Models;

public struct BBox
{
    public Vector2 Min { get; set; }
    public Vector2 Max { get; set; }

    public override string ToString()
    {
    	return $"Min: {{{Min}}}, Max: {{{Max}}}";
    }
}
