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

		    /*Console.WriteLine(string.Join("; ", _points));
		    Console.WriteLine();
		    Console.WriteLine(string.Join("; ", points));
		    Console.WriteLine();*/
		    
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
    
        var bbox = beziers.Aggregate(new BBox(), (acc, bezier) =>
        {
        	var bbox = bezier.Length == 4 ? GetCBezierBBox(bezier) :
        						 bezier.Length == 3 ? GetQBezierBBox(bezier) :
        						 GetLineBBox(bezier);

					//Console.WriteLine(bbox);
        	
          var newMin = new Vector2(MathF.Min(bbox.Min.X, acc.Min.X), MathF.Min(bbox.Min.Y, acc.Min.Y));
          var newMax = new Vector2(MathF.Max(bbox.Max.X, acc.Max.X), MathF.Max(bbox.Max.Y, acc.Max.Y));

          acc.Min = newMin;
          acc.Max = newMax;

          return acc;
        });

        //Console.WriteLine(bbox);

        var min = bbox.Min;
        min.Y -= 1;

        Position = min;
        //Size = new Vector2(MathF.Abs(bbox.Max.X - bbox.Min.X), MathF.Abs(bbox.Max.Y - bbox.Min.Y));
        Size = new Vector2(bbox.Max.X - bbox.Min.X, bbox.Max.Y - bbox.Min.Y);

        var cPositionX = (int)MathF.Ceiling(Position.X);
        var cPositionY = (int)MathF.Ceiling(Position.Y);

        var cSizeX = (int)MathF.Ceiling(Size.X);
        var cSizeY = (int)MathF.Ceiling(Size.Y);

        var aSizeX = cSizeX + 2;
        var aSizeY = cSizeY + 2;

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
            var point = GetPoint(beziers, i, MathF.Round(Position.X), MathF.Round(Position.Y) + 0.5f);
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

						//if (index >= _result.Length) continue;

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

				return new Vector2(result.X - cPositionX, result.Y - cPositionY);
    
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

    private static BBox GetLineBBox(IReadOnlyVector2<float>[] line)
    {
      var p0 = line[0];
      var p1 = line[1];

      return new BBox()
      {
        Min = new Vector2(MathF.Min(p0.X, p1.X), MathF.Min(p0.Y, p1.Y)),
        Max = new Vector2(MathF.Max(p0.X, p1.X), MathF.Max(p0.Y, p1.Y))
      };
    }

    private static BBox GetQBezierBBox(IReadOnlyVector2<float>[] bezier)
    {
			var p0 = bezier[0];
			var p1 = bezier[1];
			var p2 = bezier[2];
    
    	// Calculate the t values where the derivative is zero
 	    float tx = (p0.X - p1.X) / (p0.X - 2 * p1.X + p2.X);
 	    float ty = (p0.Y - p1.Y) / (p0.Y - 2 * p1.Y + p2.Y);
 	
 	    // Calculate the corresponding x and y coordinates
 	    float xMin = MathF.Min(p0.X, p2.X);
 	    float xMax = MathF.Max(p0.X, p2.X);
 	    float yMin = MathF.Min(p0.Y, p2.Y);
 	    float yMax = MathF.Max(p0.Y, p2.Y);
 	
 	    if (0 <= tx && tx <= 1)
 	    {
 	        float x = (1 - tx) * (1 - tx) * p0.X + 2 * (1 - tx) * tx * p1.X + tx * tx * p2.X;
 	        xMin = MathF.Min(xMin, x);
 	        xMax = MathF.Max(xMax, x);
 	    }
 	
 	    if (0 <= ty && ty <= 1)
 	    {
 	        float y = (1 - ty) * (1 - ty) * p0.Y + 2 * (1 - ty) * ty * p1.Y + ty * ty * p2.Y;
 	        yMin = MathF.Min(yMin, y);
 	        yMax = MathF.Max(yMax, y);
 	    }

 	    return new BBox()
 	    {
 	    	Min = new Vector2(xMin, yMin),
 	    	Max = new Vector2(xMax, yMax)
 	    };
    }
    
    private static BBox GetCBezierBBox(IReadOnlyVector2<float>[] bezier)
    {
			static float Bezier(float t, float p0, float p1, float p2, float p3)
			{
			    float mt = 1 - t;
			    return mt * mt * mt * p0 + 3 * mt * mt * t * p1 + 3 * mt * t * t * p2 + t * t * t * p3;
			}
    
			var p0 = bezier[0];
			var p1 = bezier[1];
			var p2 = bezier[2];
			var p3 = bezier[3];

 	    float t;
      List<float> x = new List<float>() { p0.X, p3.X };
      List<float> y = new List<float>() { p0.Y, p3.Y };
  
      // Calculate derivative for X
      float a = -3 * p0.X + 9 * p1.X - 9 * p2.X + 3 * p3.X;
      float b = 6 * p0.X - 12 * p1.X + 6 * p2.X;
      float c = -3 * p0.X + 3 * p1.X;
  
      // Calculate roots for X
      float discriminant = b * b - 4 * a * c;
      if (discriminant >= 0)
      {
          discriminant = (float)MathF.Sqrt(discriminant);
          t = (-b - discriminant) / (2 * a);
          if (t > 0 && t < 1) x.Add(Bezier(t, p0.X, p1.X, p2.X, p3.X));
          t = (-b + discriminant) / (2 * a);
          if (t > 0 && t < 1) x.Add(Bezier(t, p0.X, p1.X, p2.X, p3.X));
      }
  
      // Calculate derivative for Y
      a = -3 * p0.Y + 9 * p1.Y - 9 * p2.Y + 3 * p3.Y;
      b = 6 * p0.Y - 12 * p1.Y + 6 * p2.Y;
      c = -3 * p0.Y + 3 * p1.Y;
  
      // Calculate roots for Y
      discriminant = b * b - 4 * a * c;
      if (discriminant >= 0)
      {
          discriminant = (float)MathF.Sqrt(discriminant);
          t = (-b - discriminant) / (2 * a);
          if (t > 0 && t < 1) y.Add(Bezier(t, p0.Y, p1.Y, p2.Y, p3.Y));
          t = (-b + discriminant) / (2 * a);
          if (t > 0 && t < 1) y.Add(Bezier(t, p0.Y, p1.Y, p2.Y, p3.Y));
      }
 	
 	    // Return the bounding box
 	    return new BBox()
 	    {
 	    	Min = new Vector2(x.Min(), y.Min()),
 	    	Max = new Vector2(x.Max(), y.Max())
 	    };
 		}
 	
 		private static float[] SolveCubic(float a, float b, float c)
 		{
 	    // Calculate the discriminant
 	    float d = b * b - 4 * a * c;
 	
 	    // If the discriminant is negative, there are no real roots
 	    if (d < 0)
 	    {
 	      return new float[0];
 	    }
 	
 	    // Calculate the two roots
 	    float sqrtD = (float)MathF.Sqrt(d);
 	    float t1 = (-b - sqrtD) / (2 * a);
 	    float t2 = (-b + sqrtD) / (2 * a);
 	
 	    return new float[] { t1, t2 };
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
