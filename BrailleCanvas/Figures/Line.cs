/*import { IFigure } from './braille-canvas/interfaces.js';
import { auto, Auto } from './braille-canvas/constants.js';
import { Vector2, lerpVector2, mod, round, getDotsInCell } from './math.js';
import { Color, white, figureTStep } from './utils.js';

function replaceAt(str: string, index: number, replacement: string): string {
  return str.substr(0, index) + replacement + str.substr(index + replacement.length);
}*/

using BrailleCanvas.Models;
using BrailleCanvas.Interfaces;

namespace BrailleCanvas.Figures;

public class Line : IFigure {
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

  public Line(IEnumerable<IReadOnlyVector2<float>> points, Color color)
  {
  	this.Color = color;

  	var bbox = points.Aggregate((acc, p) => {
  		acc.Min.X = Math.Min(p.X, acc.Min.X);
  		acc.Max.X = Math.Min(p.X, acc.Max.X);
  		acc.Min.Y = Math.Min(p.Y, acc.Min.Y);
  		acc.Max.Y = Math.Min(p.Y, acc.Max.Y);

  		return acc;
  	}, new BBox());
  }

  public readonly IReadOnlyVector2<float> Size { get; private set; }
  public readonly IReadOnlyVector2<float> Position { get; private set; }
  public readonly int? ZIndex { get; private set; }
  public readonly Color Color { get; private set; }
}
