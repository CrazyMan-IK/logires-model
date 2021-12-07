using OneOf;
using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Figures;

public class Ellipse : IFilledFigure
{
    private string _result = "";
    private int _oldHash = 0;

    public Ellipse(IReadOnlyVector2<float> position, IReadOnlyVector2<float> size, bool isFilled, OneOf<Color, IHasValue<Color>> color)
    {
        IsFilled = isFilled;
        Color = color;

        Position = position;
        Size = size;
    }

    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public OneOf<Color, IHasValue<Color>> Color { get; private set; }
    public bool IsFilled { get; private set; }

    public string StringValue()
    {
        var newHash = HashCode.Combine(Size, Position);

        if (_oldHash != newHash)
        {
            _oldHash = newHash;
            Update();
        }

        return _result;
    }

    public bool IsInside(IReadOnlyVector2<float> point)
    {
        var dx = MathF.Pow(point.X - Position.X - Size.X * 0.5f, 2) / MathF.Pow(MathF.Ceiling(Size.X) * 0.5f - 0.1f, 2);
        var dy = MathF.Pow(point.Y - Position.Y - Size.Y * 0.5f, 2) / MathF.Pow(MathF.Ceiling(Size.Y) * 0.5f - 0.1f, 2);
        return dx + dy <= 1;
    }

    private void Update()
    {
        var cPositionX = MathF.Ceiling(Position.X);
        var cPositionY = MathF.Ceiling(Position.Y);

        var cSizeX = MathF.Ceiling(Size.X);
        var cSizeY = MathF.Ceiling(Size.Y);

        for (int i = 0; i < cSizeY + 1 + cPositionY; i++)
        {
            for (int j = 0; j < cSizeX + 1 + cPositionX; j++)
            {
                _result += '\u2800';
            }
            _result += '\n';
        }

        var hSize = Size * 0.5f;

        var accum = 0;
        for (float i = 0; i <= 1; i += Constants.FigureTimeStep)
        {
            var rad = MathExtensions.Lerp(0, MathF.Tau, i);
            var curX = Position.X + hSize.X + MathF.Sin(rad) * hSize.X;
            var curY = Position.Y + hSize.Y + MathF.Cos(rad) * -hSize.Y;
            var point = new Vector2(curX, curY);

            if (point.X < 0 || point.Y < 0)
            {
                continue;
            }

            var rX = MathExtensions.Round(point.X, 4);
            var rY = MathExtensions.Round(point.Y, 4);

            accum |= (int)(new Cell(new Vector2(rX - 0.5f, rY - 0.5f)).State);

            var cellX = MathF.Max(MathExtensions.Round(rX), 0);
            var cellY = MathF.Max(MathExtensions.Round(rY), 0);

            var index = (int)MathF.Truncate(cellX + cellY * (cSizeX + 1 + cPositionX) + cellY);

            var oldCode = _result[index] - 0x2800;
            var old = oldCode >= 0x00 && oldCode <= 0xff ? oldCode : 0;
            //Console.WriteLine(new { curX, curY, cellX, cellY, index, old, oldCode, accum });
            //Console.WriteLine();
            accum |= old;
            _result = _result.ReplaceAt(index, (char)(0x2800 + accum));

            accum = 0;
        }
    }
}