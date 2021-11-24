using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var rows = 20;
var columns = 76;

var c = new Canvas(new Vector2Int(columns, rows));
var ln1 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, 1), new Vector2(columns - 1, rows - 1) }, new Color(255, 0, 0));
Console.WriteLine();
var ln2 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, rows - 1), new Vector2(columns - 1, 1) }, new Color(0, 0, 255));
var el1 = new Ellipse(new Vector2(columns / 2, rows / 2), new Vector2(9, 9), true, new Color(0, 0, 255));

c.Append(ln1);
c.Append(ln2);
c.Append(el1);

Console.Write("|----------------------------------------------------|");
Console.Write(c.StringValue());
Console.Write("|----------------------------------------------------|");
