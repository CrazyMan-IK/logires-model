using BrailleCanvas.Models;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;

namespace BrailleCanvas.Figures;

public class Ellipse : IFilledFigure
{
    /*
	private _result = '';
	  private _width = 0;
	  private _height = 0;
	  private _position: Vector2 = { x: 0, y: 0 };
	  private _zIndex: Auto = auto;
	  private _isFilled = true;
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
	
	  public get isFilled(): boolean {
	    return this._isFilled;
	  }
	
	  public get color(): Color {
	    return this._color;
	  }
	
	  public constructor(position: Vector2, size: Vector2, filled = true, color = white) {
	    this._isFilled = filled;
	    this._color = color;
	    //console.log(end);
	
	    const cPositionX = Math.ceil(position.x);
	    const cPositionY = Math.ceil(position.y);
	
	    const cSizeX = Math.ceil(size.x);
	    const cSizeY = Math.ceil(size.y);
	
	    this._position = position;
	
	    this._width = size.x;
	    this._height = size.y;
	
	    for (let i = 0; i < cSizeY + 1 + cPositionY; i++) {
	      for (let j = 0; j < cSizeX + 1 + cPositionX; j++) {
	        this._result += '\u2800'; //filled && this.isInside({ x: j, y: i }) ? ' ' : '\u2800';
	      }
	      this._result += '\n';
	    }
	
	    //console.log(this._result.length);
	
	    size.x *= 0.5;
	    size.y *= 0.5;
	
	    //let prevX = 0;
	    //let prevY = 0;
	    //const prevIndex = -1;
	    let accum = 0;
	    for (let i = 0; i <= 1; i += figureTStep) {
	      const rad = lerp(0, Math.PI * 2, i);
	      const curX = position.x + size.x + Math.sin(rad) * size.x;
	      const curY = position.y + size.y + Math.cos(rad) * -size.y;
	
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
	      //console.log({ curX, curY, cellX, cellY, index, old, oldCode, accum });
	      //console.log();
	      accum |= old;
	      this._result = replaceAt(this._result, index, String.fromCharCode(0x2800 + accum));
	
	      accum = 0;
	    }
	  }
	
	  public isInside(point: Vector2): boolean {
	    const dx = Math.pow(point.x - this._position.x - this._width * 0.5, 2) / Math.pow(this.width * 0.5 - 0.1, 2);
	    const dy = Math.pow(point.y - this._position.y - this._height * 0.5, 2) / Math.pow(this.height * 0.5 - 0.1, 2);
	    return dx + dy <= 1;
	  }
	
	  public stringValue(): string {
	    return this._result;
	  }
	*/

    private string _result = "";

    public Ellipse(IReadOnlyVector2<float> position, IReadOnlyVector2<float> size, bool isFilled, Color color)
    {
        IsFilled = isFilled;
        Color = color;

        Position = position;
        Size = size;

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

		size *= 0.5f;

		var accum = 0;
        for (float i = 0; i <= 1; i += Constants.FigureTimeStep)
        {
            var rad = MathExtensions.Lerp(0, MathF.Tau, i);
            var curX = position.X + size.X + MathF.Sin(rad) * size.X;
            var curY = position.Y + size.Y + MathF.Cos(rad) * -size.Y;
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

            //const index = Math.trunc(cellX + cellY * (size.x * 2 + 1) + cellY);
            var oldCode = _result[index] - 0x2800;
            var old = oldCode >= 0x00 && oldCode <= 0xff ? oldCode : 0;
			//Console.WriteLine(new { curX, curY, cellX, cellY, index, old, oldCode, accum });
            //Console.WriteLine();
            accum |= old;
            _result = _result.ReplaceAt(index, (char)(0x2800 + accum));

            accum = 0;
        }
    }

    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public Color Color { get; private set; }
    public bool IsFilled { get; private set; }

    public string StringValue()
    {
        return _result;
    }

    public bool IsInside(IReadOnlyVector2<float> point)
    {
        var dx = MathF.Pow(point.X - Position.X - Size.X * 0.5f, 2) / MathF.Pow(MathF.Ceiling(Size.X) * 0.5f - 0.1f, 2);
        var dy = MathF.Pow(point.Y - Position.Y - Size.Y * 0.5f, 2) / MathF.Pow(MathF.Ceiling(Size.Y) * 0.5f - 0.1f, 2);
        return dx + dy <= 1;
    }
}
