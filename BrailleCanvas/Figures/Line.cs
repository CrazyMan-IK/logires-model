using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;
using OneOf;

namespace BrailleCanvas.Figures;

public class Line : IFigure
{
    private char[] _result = Array.Empty<char>();
    private int _oldPointsHash = 0;

    public Line(IEnumerable<IReadOnlyVector2<float>> points, OneOf<Color, Ternary<Color>> color)
    {
        Points = points;
        Color = color;

        Size = Vector2.Zero;
        Position = Vector2.Zero;
    }

    public IEnumerable<IReadOnlyVector2<float>> Points { get; private set; }
    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public OneOf<Color, Ternary<Color>> Color { get; private set; }

    public string StringValue()
    {
        var newHash = GetPointsHashCode();

        if (_oldPointsHash != newHash)
        {
            _oldPointsHash = newHash;
            Update();
        }

        //Update();

        return string.Join("", _result);
    }

    private int GetPointsHashCode()
    {
        return Points.Aggregate(0, (acc, p) => acc + p.GetHashCode());
    }

    private void Update()
    {
        var bbox = Points.Aggregate(new BBox(), (acc, p) =>
        {
            var newMin = new Vector2(Math.Min(p.X, acc.Min.X), Math.Min(p.Y, acc.Min.Y));
            var newMax = new Vector2(Math.Max(p.X, acc.Max.X), Math.Max(p.Y, acc.Max.Y));

            acc.Min = newMin;
            acc.Max = newMax;

            return acc;
        });

        Position = bbox.Min;
        Size = new Vector2(MathF.Abs(bbox.Max.X - bbox.Min.X), MathF.Abs(bbox.Max.Y - bbox.Min.Y));

        var cPositionX = (int)MathF.Ceiling(Position.X);
        var cPositionY = (int)MathF.Ceiling(Position.Y);

        var cSizeX = (int)MathF.Ceiling(Size.X);
        var cSizeY = (int)MathF.Ceiling(Size.Y);

        var aSizeX = cSizeX + 2 + cPositionX;
        var aSizeY = cSizeY + 1 + cPositionY;

        _result = new char[aSizeX * aSizeY];
        for (int i = 0; i < aSizeY; i++)
        {
            for (int j = 0; j < aSizeX - 1; j++)
            {
                _result[i * aSizeX + j] = '\u2800'; //filled && this.isInside({ x: j, y: i })
            }
            _result[i * aSizeX + (aSizeX - 1)] = '\n';
        }

        //row * rows + column
        //i * aSizeY + j

        //Console.WriteLine(bbox);

        var pointsList = Points.ToList();
        var accum = 0;
        for (float i = 0; i <= pointsList.Count - 1; i += Constants.FigureTimeStep)
        {
            var point = GetPoint(pointsList, i);
            //Console.WriteLine(point);
            //const curY = lerp(start.y, end.y, i);

            if (point.X < 0 || point.Y < 0)
            {
                continue;
            }

            var rX = MathExtensions.Round(point.X, 4);
            var rY = MathExtensions.Round(point.Y, 4);
            //Console.WriteLine(new Vector2(rX - 0.5f, rY - 0.5f));

            accum |= (int)Cell.GetStateByPosition(new Vector2(rX - 0.5f, rY - 0.5f));

            var cellX = MathF.Max(MathExtensions.Round(rX), 0);
            var cellY = MathF.Max(MathExtensions.Round(rY), 0);
            //Console.WriteLine(new Vector2(cellX, cellY));

            var index = (int)MathF.Truncate(cellX + cellY * (cSizeX + 1 + cPositionX) + cellY);

            //const index = Math.trunc(cellX + cellY * (size.x * 2 + 1) + cellY);
            var oldCode = _result[index] - 0x2800;
            var old = oldCode >= 0x00 && oldCode <= 0xff ? oldCode : 0;
            //console.log({ curX, curY, cellX, cellY, index, old, oldCode, accum }
            //console.log();
            accum |= old;
            _result[index] = (char)(0x2800 + accum);

            accum = 0;
        }
    }

    private static IReadOnlyVector2<float> GetPoint(List<IReadOnlyVector2<float>> points, float i)
    {
        var p1 = points[(int)MathF.Floor(i)];
        var p2 = points[(int)MathF.Floor(i) + 1];

        return Vector2Extensions.Lerp(p1, p2, MathExtensions.Mod(i, 1 + Constants.FigureTimeStep));
    }
}