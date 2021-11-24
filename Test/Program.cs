using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var rows = 20;
var columns = 76;

var hrows = rows / 2;
var hcolumns = columns / 2;

while (true)
{
    var t = ((float)DateTime.Now.TimeOfDay.TotalSeconds);
    var p1 = new Vector2(hcolumns + MathF.Sin(t) * hcolumns, hrows + MathF.Cos(t) * hrows);
    var p2 = new Vector2(hcolumns + MathF.Sin(t + MathF.PI) * hcolumns, hrows + MathF.Cos(t + MathF.PI) * hrows);
    var p3 = new Vector2(hcolumns + MathF.Cos(t) * hcolumns, hrows + MathF.Sin(t) * hrows);
    var p4 = new Vector2(hcolumns + MathF.Cos(t + MathF.PI) * hcolumns, hrows + MathF.Sin(t + MathF.PI) * hrows);

    var c = new Canvas(new Vector2Int(columns, rows));
    /*var ln1 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, 1), new Vector2(columns - 1, rows - 1) }, new Color(255, 0, 0));
    var ln2 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, rows - 1), new Vector2(columns - 1, 1) }, new Color(0, 0, 255));*/
    var ln1 = new Line(new List<IReadOnlyVector2<float>> { p1, p2 }, new Color(255, 0, 0));
    var ln2 = new Line(new List<IReadOnlyVector2<float>> { p3, p4 }, new Color(0, 0, 255));
    var el1 = new Ellipse(new Vector2(columns / 2 - 9, rows / 2 - 4.5f), new Vector2(18, 9), true, new Color(0, 255, 0));

    c.Append(ln1);
    c.Append(ln2);
    c.Append(el1);

    Console.SetCursorPosition(0, 0);

    Console.WriteLine("|----------------------------------------------------|");
    Console.WriteLine(c.StringValue());
    Console.WriteLine("|----------------------------------------------------|");
}
