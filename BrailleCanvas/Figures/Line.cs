/*import { IFigure } from './braille-canvas/interfaces.js';
import { auto, Auto } from './braille-canvas/constants.js';
import { Vector2, lerpVector2, mod, round, getDotsInCell } from './math.js';
import { Color, white, figureTStep } from './utils.js';

function replaceAt(str: string, index: number, replacement: string): string {
  return str.substr(0, index) + replacement + str.substr(index + replacement.length);
}*/

using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;

namespace BrailleCanvas.Figures;

public class Line : IFigure
{
    /*private _result = '';
    private _width = 0;
    private _height = 0;
    private _position: Vector2 = { x: 0, y: 0 };
    private _zIndex: Auto = auto;
    private _color: Color = white;

    public get width(): number {
      return Math.ceil(this._width);
    }

    public get height(): number {
      return Math.ceil(this._height);
    }

    public get position(): Vector2 {
      return { x: Math.ceil(this._position.x), y: Math.ceil(this._position.y) };
    }

    public get zIndex(): number | Auto {
      return this._zIndex;
    }

    public get color(): Color {
      return this._color;
    }

    //public constructor(start: Vector2, end: Vector2, color = white) {
    public constructor(points: Vector2[], color = white) {
      this._color = color;

      const bbox = points.reduce(
        (acc, p) => {
          acc.min.x = Math.min(p.x, acc.max.x);
          acc.max.x = Math.max(p.x, acc.max.x);
          acc.min.y = Math.min(p.y, acc.min.y);
          acc.max.y = Math.max(p.y, acc.max.y);

          return acc;
        },
        { 
        min: { x: Number.MAX_VALUE, y: Number.MAX_VALUE }, 
        max: { x: Number.MIN_VALUE, y: Number.MIN_VALUE } 
        }
      );

      const getP = (i: number) => {
        const p1 = points[Math.floor(i)];
        const p2 = points[Math.floor(i) + 1];
        return lerpVector2(p1, p2, mod(i, 1 + figureTStep));
      };

      this._position = bbox.min;
      this._width = Math.abs(bbox.max.x - bbox.min.x);
      this._height = Math.abs(bbox.max.y - bbox.min.y);

      const cPositionX = this.position.x;
      const cPositionY = this.position.y;

      const cSizeX = this.width;
      const cSizeY = this.height;

      for (let i = 0; i < cSizeY + 1 + cPositionY; i++) {
        for (let j = 0; j < cSizeX + 1 + cPositionX; j++) {
          this._result += '\u2800'; //filled && this.isInside({ x: j, y: i })
        }
        this._result += '\n';
      }

      let accum = 0;
      for (let i = 0; i <= points.length - 1; i += figureTStep) {
        const { x: curX, y: curY } = getP(i);
        //const curY = lerp(start.y, end.y, i);

        if (curX < 0 || curY < 0) {
          continue;
        }

        const rX = round(curX, 4);
        const rY = round(curY, 4);

        accum |= getDotsInCell(rX - 0.5, rY - 0.5);

        const cellX = Math.max(Math.round(rX), 0);
        const cellY = Math.max(Math.round(rY), 0);

        const index = Math.trunc(cellX + cellY * (cSizeX + 1 + cPositionX) + cellY);

        //const index = Math.trunc(cellX + cellY * (size.x * 2 + 1) + cellY);
        const oldCode = this._result.charCodeAt(index) - 0x2800;
        const old = oldCode >= 0x00 && oldCode <= 0xff ? oldCode : 0;
        //console.log({ curX, curY, cellX, cellY, index, old, oldCode, accum }
        //console.log();
        accum |= old;
        this._result = replaceAt(this._result, index, String.fromCharCode(0x2800 + accum));

        accum = 0;
      }
    }

    public stringValue(): string {
      return this._result;
    }*/

    private char[] _result = Array.Empty<char>();
    private int _oldPointsHash = 0;

    public Line(IEnumerable<IReadOnlyVector2<float>> points, Color color)
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
    public Color Color { get; private set; }

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

            /*acc.Min.X = Math.Min(p.X, acc.Min.X);
            acc.Max.X = Math.Max(p.X, acc.Max.X);
            acc.Min.Y = Math.Min(p.Y, acc.Min.Y);
            acc.Max.Y = Math.Max(p.Y, acc.Max.Y);*/

            //TODO: 123
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

            accum |= (int)(new Cell(new Vector2(rX - 0.5f, rY - 0.5f)).State);

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

    private IReadOnlyVector2<float> GetPoint(List<IReadOnlyVector2<float>> points, float i)
    {
        var p1 = points[(int)MathF.Floor(i)];
        var p2 = points[(int)MathF.Floor(i) + 1];

        return Vector2Extensions.Lerp(p1, p2, MathExtensions.Mod(i, 1 + Constants.FigureTimeStep));
    }
}