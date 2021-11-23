using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var c = new Canvas(new Vector2Int(80, 20));
var ln1 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(0, 0), new Vector2(80, 20) }, new Color(255, 0, 0));
var ln2 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(0, 20), new Vector2(80, 0) }, new Color(0, 255, 0));

c.Append(ln1);
c.Append(ln2);

Console.Write(c.StringValue());
