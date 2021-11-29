using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Figures;

public class Item : IFilledFigure
{
    private int _oldHash = 0;

    public Item(string text, IReadOnlyVector2<float> position, bool isFilled, Color color)
    {
        Text = text;
        Size = Vector2.Zero;
        Position = position;
        Color = color;
        IsFilled = isFilled;
    }

    public string Text { get; set; }
    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public Color Color { get; private set; }
    public bool IsFilled { get; private set; }

    public string StringValue()
    {
        var newHash = Text.GetHashCode();

        if (_oldHash != newHash)
        {
            _oldHash = newHash;
            Update();
        }

        return Text;
    }

    public bool IsInside(IReadOnlyVector2<float> point)
    {
        return point.X > Position.X && point.X < Position.X + Size.X &&
                     point.Y > Position.Y && point.Y < Position.Y + Size.Y;
    }

    private void Update()
    {
        var lines = Text.Split("\n");

        var x = lines.Select((line) => line.Length).Max();
        var y = lines.Length;

        Size = new Vector2(x, y);
    }
}
