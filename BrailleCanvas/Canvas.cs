using System.Linq;
using BrailleCanvas.Models;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;

﻿namespace BrailleCanvas;

public class Canvas
{
	private readonly List<(int, IFigure)> _items = new List<(int, IFigure)>();
	private int _topZ = 0;
	private bool _canOverflow = false;
	
	public Canvas(IReadOnlyVector2<int> size, bool canOverflow = false)
	{
		Size = size;
		_canOverflow = canOverflow;
	}

	public IReadOnlyVector2<int> Size { get; private set; }

	public void Append(IFigure item)
	{
	  int z;
	  if (item.ZIndex == null)
	  {
	    _topZ++;
	    z = _topZ;
	  }
	  else
	  {
	    _topZ = z = item.ZIndex.Value;
	  }
	  
	  _items.Add((z, item));
	}

	public string StringValue()
	{
		Sort();
		var matrix = new string[Size.Y, Size.X];
		var cmatrix = new List<Color>[Size.Y, Size.X];

		foreach (var (z, item) in _items)
		{
			var lines = item.StringValue().Split("\n");
		  
		  //var x = item.Position.X * 0;
		  //var y = item.Position.Y * 0;
		  var y = 0;
		  
		  foreach (var line in lines)
		  {
		  	Draw(item, matrix, cmatrix, line, 0, y++);
		  }
		}

		for (int y = 0; y < matrix.GetLength(0); y++)
		{
		  for (int x = 0; x < matrix.GetLength(1); x++)
		  {
		    if (matrix[y, x] == null)
		    {
		      continue;
		    }
		
		    var colors = cmatrix[y, x];
		    var color = colors[0]; //mixColors(colors);
		
		    if (colors.Count > 0)
		    {
		      //console.log(getColorEscapeCharacter(color));
		      //matrix[y][x] = getColorEscapeCharacter(color) + matrix[y][x];
		    }
		  }
		}

		//var tst = from item in matrix
		//					select item;

		//Console.WriteLine(tst.GetType());
		//Console.WriteLine(matrix.Cast<string>().ToList().GetType());

		//return "";
		return string.Join("\n", matrix.GetRows().Select((row) => string.Join("", row)));
	}

	private void Sort()
	{
		_items.Sort((a, b) => a.Item1 - b.Item1);
	}

	private bool IsInBounds(int x)
	{
		return x < Size.X || _canOverflow;
	}

	private void Draw(IFigure item, string[,] matrix, List<Color>[,] cmatrix, string str, int x, int y)
	{
		if (y < 0 || (y > Size.Y - 1 && !_canOverflow))
		{
		  return;
		}
		
		var i = x;
		if (x > 0)
		{
			for (int i = x; i > 0; i--)
			{
		    if (!matrix[y, i] && IsInBounds(i))
		    {
		      matrix[y, i] = ' ';
		    }
		  }
		}

		for (const chr of str)
		{
      if (!IsInBounds(i))
      {
        break;
      }

      var chCodeA = chr.charCodeAt(0);
      var chCodeB = (matrix[y][i] ?? '\u2800').charCodeAt(0);

      var result = '';
      if (chCodeA >= 0x2800 && chCodeA <= 0x28ff && chCodeB >= 0x2800 && chCodeB <= 0x28ff)
      {
        if (chCodeB !== 0x2800 && isObjectImplementFilledFigure(item) && item.isFilled)
        {
          //const isInside = item.isInside.bind(item);

          const dots = getDotsPositionsInCell(i, y);
          const chCodeC =
            0x2800 +
            applyPredicateToDots(chCodeA - 0x2800, (chCodeA | chCodeB) - 0x2800, (a, b, index) => {
              //if (chCodeB !== 0x2800) console.log({ a, b, index, ii: isInside(dots[7 - index]) }); //, dots });
              return item.isInside(dots[7 - index]);
            });
          //if (chCodeB !== 0x2800) console.log({ ac: chCodeA.toString(16), bc: chCodeB.toString(16) });

          result = String.fromCharCode(chCodeC);
        } else {
          result = String.fromCharCode(chCodeA | chCodeB);
        }
      } else {
        result = chr;
      }

      matrix[y][i] = result;

      if (chCodeA !== 0x2800)
      {
        //console.log(`"${result}" "x: ${i}"\t"y: ${y}"\t"${JSON.stringify(item.color)}"`);
        cmatrix[y][i].push(item.color);
      }

      /*if (item.isInside({ x: i, y: y })) {
        matrix[y][i] = chr;
      } else {
        matrix[y][i] = String.fromCharCode(chCodeA | chCodeB);
      }*/

      i++;
    }
	}
}
