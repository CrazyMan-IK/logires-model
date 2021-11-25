using BrailleCanvas;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Models;
using System.Text;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var rows = 20;
var columns = 76;

var hrows = rows / 2;
var hcolumns = columns / 2;

var c = new Canvas(new Vector2Int(columns, rows));

var t = 0f;
var p1 = new RefVector2();
var p2 = new RefVector2();
var p3 = new RefVector2();
var p4 = new RefVector2();

var plist1 = new List<IReadOnlyVector2<float>> { p1, p2 };
var plist2 = new List<IReadOnlyVector2<float>> { p3, p4 };

float UpdateVectors()
{
    var oldT = t;
    t = ((float)DateTime.Now.TimeOfDay.TotalSeconds) / 2;

    p1.X = hcolumns + MathF.Sin(t) * hcolumns;
    p1.Y = hrows + MathF.Cos(t) * hrows;

    p2.X = hcolumns + MathF.Sin(t + MathF.PI) * hcolumns;
    p2.Y = hrows + MathF.Cos(t + MathF.PI) * hrows;

    p3.X = hcolumns + MathF.Cos(t) * hcolumns;
    p3.Y = hrows + MathF.Sin(t) * hrows;

    p4.X = hcolumns + MathF.Cos(t + MathF.PI) * hcolumns;
    p4.Y = hrows + MathF.Sin(t + MathF.PI) * hrows;

    return t - oldT;
}
UpdateVectors();

string CreateFrame()
{
    var builder = new StringBuilder(columns * rows);

    var xCount = columns - 2;
    var yCount = rows - 2;

    builder.Append('+');
    builder.Append(new string('-', xCount));
    builder.Append("+\n");

    for (int i = 0; i < yCount; i++)
    {
        builder.Append('|');
        builder.Append(new string('\u2800', xCount));
        builder.Append("|\n");
    }

    builder.Append('+');
    builder.Append(new string('-', xCount));
    builder.Append("+\n");

    return builder.ToString();
}

var ln1 = new Line(plist1, new Color(255, 0, 0));
var ln2 = new Line(plist2, new Color(0, 0, 255));
var el1 = new Ellipse(new Vector2(columns / 2 - 9, rows / 2 - 4.5f), new Vector2(18, 9), true, new Color(0, 255, 0));
var frame = new Item(CreateFrame(), Vector2.Zero, false, Color.White);

c.Append(ln1);
c.Append(ln2);
c.Append(el1);
c.Append(frame);

while (true)
{
    var dt = UpdateVectors();
    /*var ln1 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, 1), new Vector2(columns - 1, rows - 1) }, new Color(255, 0, 0));
    var ln2 = new Line(new List<IReadOnlyVector2<float>> { new Vector2(1, rows - 1), new Vector2(columns - 1, 1) }, new Color(0, 0, 255));*/

    //c.Clear();

    Console.SetCursorPosition(0, 0);

    Console.WriteLine(c.StringValue());
    Console.WriteLine(1 / dt);
}