using System.Runtime.InteropServices;
using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;
using OneOf;

namespace BrailleCanvas.Figures;

public class Bezier : IFigure
{
    private readonly List<IReadOnlyVector2<float>> _points = new List<IReadOnlyVector2<float>>();
    private char[] _result = Array.Empty<char>();
    private string _text = "";
    private int _oldPointsHash = 0;
    //private bool _isFirstTime = true;

    public Bezier(IEnumerable<IReadOnlyVector2<float>> points, OneOf<Color, IHasValue<Color>> color, bool isSmoothed = true)
    {
        _points.AddRange(points);
        Color = color;
        IsSmoothed = isSmoothed;

        Size = Vector2.Zero;
        Position = Vector2.Zero;
    }

    public IEnumerable<IReadOnlyVector2<float>> Points => _points;
    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public OneOf<Color, IHasValue<Color>> Color { get; private set; }
    public bool IsSmoothed { get; private set; }

    public void SetPoints(IEnumerable<IReadOnlyVector2<float>> points)
    {
    		_points.Clear();
        _points.AddRange(points);

        //_isFirstTime = true;
    }

    public string StringValue()
    {
        var newHash = GetPointsHashCode();

        if (_oldPointsHash != newHash)
        {
            _oldPointsHash = newHash;
            Update();
        }

        return _text;
    }

    private int GetPointsHashCode()
    {
    		var result = 0;
				for (int i = 0; i < _points.Count; i++)
				{
					result += _points[i].GetHashCode();
				}

    		return result;
    }

    private void Update()
    {
		    /*if (_isFirstTime &&	IsSmoothed)
		    {
		    	for (int i = 3; i < _points.Count; i += 3)
		    	{
		    		var ps = _points.Skip(i - 1).Take(5).ToArray();
		    		if (ps.Length > 2 && ps.Length < 5)
		    		{
		    			var newPoint = new Vector2((ps[0].X + ps[1].X) * 0.5f, (ps[0].Y + ps[1].Y) * 0.5f);
		    			_points.Insert(i, newPoint);
		    		}
		    	}

		    	_isFirstTime = false;
		    }*/

		    var points = _points;
		    if (IsSmoothed)
		    {
		    	points = new List<IReadOnlyVector2<float>>(_points);

		    	for (int i = 3; i < points.Count; i += 3)
		    	{
		    		var ps = points.Skip(i - 1).Take(5).ToArray();
		    		if (ps.Length > 2 && ps.Length < 5)
		    		{
		    			var newPoint = new Vector2((ps[0].X + ps[1].X) * 0.5f, (ps[0].Y + ps[1].Y) * 0.5f);
		    			points.Insert(i, newPoint);
		    		}
		    	}
		    }
		    
		    var beziers = new List<IReadOnlyVector2<float>[]>();
		    for (int i = 0; i < points.Count; i += 3)
		    {
		    	var ps = points.Skip(i).Take(4).ToArray();

		    	if (ps.Length < 2)
		    	{
		    		break;
		    	}

		    	beziers.Add(ps);
		    }
    
        var bbox = points.Aggregate(new BBox(), (acc, p) =>
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

        var aSizeX = cSizeX + 2;
        var aSizeY = cSizeY + 1;

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

        var accum = 0;
        for (float i = 0; i <= beziers.Count; i += Constants.FigureTimeStep)
        {
            var point = GetPoint(beziers, i, cPositionX, cPositionY);
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

            var index = (int)MathF.Truncate(cellX + cellY * (cSizeX + 1) + cellY);

            //const index = Math.trunc(cellX + cellY * (size.x * 2 + 1) + cellY);
            var oldCode = _result[index] - 0x2800;
            var old = oldCode >= 0x00 && oldCode <= 0xff ? oldCode : 0;
            //console.log({ curX, curY, cellX, cellY, index, old, oldCode, accum }
            //console.log();
            accum |= old;
            _result[index] = (char)(0x2800 + accum);

            accum = 0;
        }

        _text = string.Join("", _result);
    }

    private static IReadOnlyVector2<float> GetPoint(List<IReadOnlyVector2<float>[]> beziers, float i, float cPositionX = 0, float cPositionY = 0)
    {
				var bezier = beziers[(int)MathF.Floor(i)];
				var t = MathExtensions.Mod(i, 1 + Constants.FigureTimeStep);

    		IReadOnlyVector2<float> result = GetBezierPoint(bezier, t);

				return new Vector2(result.X - cPositionX, result.Y - cPositionY);;
    
        /*var p1 = points[(int)MathF.Floor(i)];
        var p2 = points[(int)MathF.Floor(i) + 1];

        var result = Vector2Extensions.Lerp(p1, p2, MathExtensions.Mod(i, 1 + Constants.FigureTimeStep));

        return new Vector2(result.X - cPositionX, result.Y - cPositionY);*/
    }

    private static IReadOnlyVector2<float> GetBezierPoint(IReadOnlyVector2<float>[] bezier, float t)
    {
    	if (bezier.Length == 2)
			{
				var p1 = bezier[0];
				var p2 = bezier[1];
			
				return Vector2Extensions.Lerp(p1, p2, t);
			}
			else if (bezier.Length == 3)
			{
				var p1 = bezier[0];
				var p2 = bezier[1];
				var p3 = bezier[2];

				return p1.Multiply(MathF.Pow(1 - t, 2)) +
							 p2.Multiply(2 * t * (1 - t)) +
							 p3.Multiply(MathF.Pow(t, 2));
			}
			else if (bezier.Length == 4)
			{
				var p1 = bezier[0];
				var p2 = bezier[1];
				var p3 = bezier[2];
				var p4 = bezier[3];

				return p1.Multiply(MathF.Pow(1 - t, 3)) +
							 p2.Multiply(3 * t * MathF.Pow(1 - t, 2)) +
							 p3.Multiply(3 * MathF.Pow(t, 2) * (1 - t)) +
							 p4.Multiply(MathF.Pow(t, 3));
			}
			
			throw new ArgumentOutOfRangeException(nameof(bezier));
    }

    /*
		function evalBez(poly, t) {
		    var x = poly[0] * (1 - t) * (1 - t) * (1 - t) + 3 * poly[1] * t * (1 - t) * (1 - t) + 3 * poly[2] * t * t * (1 - t) + poly[3] * t * t * t;
		    return x;
		}
    
		function findBB() {
		    var a = 3 * P[3].X - 9 * P[2].X + 9 * P[1].X - 3 * P[0].X;
		    var b = 6 * P[0].X - 12 * P[1].X + 6 * P[2].X;
		    var c = 3 * P[1].X - 3 * P[0].X;
		    //alert("a "+a+" "+b+" "+c);
		    var disc = b * b - 4 * a * c;
		    var xl = P[0].X;
		    var xh = P[0].X;
		    if (P[3].X < xl) xl = P[3].X;
		    if (P[3].X > xh) xh = P[3].X;
		    if (disc >= 0) {
		        var t1 = (-b + Math.sqrt(disc)) / (2 * a);
		        alert("t1 " + t1);
		        if (t1 > 0 && t1 < 1) {
		            var x1 = evalBez(PX, t1);
		            if (x1 < xl) xl = x1;
		            if (x1 > xh) xh = x1;
		        }
		
		        var t2 = (-b - Math.sqrt(disc)) / (2 * a);
		        alert("t2 " + t2);
		        if (t2 > 0 && t2 < 1) {
		            var x2 = evalBez(PX, t2);
		            if (x2 < xl) xl = x2;
		            if (x2 > xh) xh = x2;
		        }
		    }
		
		    a = 3 * P[3].Y - 9 * P[2].Y + 9 * P[1].Y - 3 * P[0].Y;
		    b = 6 * P[0].Y - 12 * P[1].Y + 6 * P[2].Y;
		    c = 3 * P[1].Y - 3 * P[0].Y;
		    disc = b * b - 4 * a * c;
		    var yl = P[0].Y;
		    var yh = P[0].Y;
		    if (P[3].Y < yl) yl = P[3].Y;
		    if (P[3].Y > yh) yh = P[3].Y;
		    if (disc >= 0) {
		        var t1 = (-b + Math.sqrt(disc)) / (2 * a);
		        alert("t3 " + t1);
		
		        if (t1 > 0 && t1 < 1) {
		            var y1 = evalBez(PY, t1);
		            if (y1 < yl) yl = y1;
		            if (y1 > yh) yh = y1;
		        }
		
		        var t2 = (-b - Math.sqrt(disc)) / (2 * a);
		        alert("t4 " + t2);
		
		        if (t2 > 0 && t2 < 1) {
		            var y2 = evalBez(PY, t2);
		            if (y2 < yl) yl = y2;
		            if (y2 > yh) yh = y2;
		        }
		    }
		
		    ctx.lineWidth = 1;
		    ctx.beginPath();
		    ctx.moveTo(xl, yl);
		    ctx.lineTo(xl, yh);
		    ctx.lineTo(xh, yh);
		    ctx.lineTo(xh, yl);
		    ctx.lineTo(xl, yl);
		    ctx.stroke();
		
		    alert("" + xl + " " + xh + " " + yl + " " + yh);
		}
    */
}
