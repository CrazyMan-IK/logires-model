using BrailleCanvas.Models;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;

namespace BrailleCanvas;

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
        var matrix = new Cell[Size.Y, Size.X];
        var cmatrix = new List<Color>[Size.Y, Size.X];
        for (int i = 0; i < cmatrix.GetLength(0); i++)
        {
            for (int j = 0; j < cmatrix.GetLength(1); j++)
            {
                cmatrix[i, j] = new List<Color>();
            }
        }

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
                var color = Color.MixColors(colors);
                //var color = colors[0];

                if (colors.Count > 0)
                {
                    //console.log(getColorEscapeCharacter(color));
                    //matrix[y][x] = getColorEscapeCharacter(color) + matrix[y][x];
                    matrix[y, x].Color = color;
                }
            }
        }

        //var tst = from item in matrix
        //					select item;

        //Console.WriteLine(tst.GetType());
        //Console.WriteLine(matrix.Cast<string>().ToList().GetType());

        //return "";
        return string.Join("\n", matrix.GetRows().Select((row) => string.Join("", row.Select(c => c?.StringValue() ?? ""))));
    }

    private void Sort()
    {
        _items.Sort((a, b) => a.Item1 - b.Item1);
    }

    private bool IsInBounds(int x)
    {
        return x < Size.X || _canOverflow;
    }

    private void Draw(IFigure item, Cell[,] matrix, List<Color>[,] cmatrix, string str, int x, int y)
    {
        if (y < 0 || (y > Size.Y - 1 && !_canOverflow))
        {
            return;
        }

        if (x > 0)
        {
            for (int i = x; i > 0; i--)
            {
                if (matrix[y, i] == null && IsInBounds(i))
                {
                    matrix[y, i] = new Cell(' ');
                }
            }
        }

        foreach (var chr in str)
        {
            if (!IsInBounds(x))
            {
                break;
            }

            //var chCodeA = (int)chr;//chr.charCodeAt(0);
            //var chCodeB = (matrix[y, x]?.GetCharCode() ?? 0x2800);//.charCodeAt(0);
            var oldChr = matrix[y, x]?.Char ?? '\u2800';

            var result = new Cell();
            if (chr >= 0x2800 && chr <= 0x28ff && oldChr >= 0x2800 && oldChr <= 0x28ff)
            {
                if (oldChr != 0x2800 && item is IFilledFigure fitem && fitem.IsFilled)
                {
                    var dots = Cell.GetDotsPositions(new Vector2Int(x, y));
                    result.Merge((char)(chr - 0x2800), (a, b, index) =>
                    {
                        return false;
                    });
                    result.Merge(matrix[y, x], (a, b, index) =>
                    {
                        return fitem.IsInside(dots[7 - index]);
                    });
                    /*const chCodeC =
                      0x2800 +
                      applyPredicateToDots(chCodeA - 0x2800, (chCodeA | chCodeB) - 0x2800, (a, b, index) => {
                        //if (chCodeB !== 0x2800) console.log({ a, b, index, ii: isInside(dots[7 - index]) }); //, dots });
                        return fitem.IsInside(dots[7 - index]);
                      });
                    //if (chCodeB !== 0x2800) console.log({ ac: chCodeA.toString(16), bc: chCodeB.toString(16) });

                    result = String.fromCharCode(chCodeC);*/
                }
                else
                {
                    result.Char = (char)(chr | oldChr);
                }
            }
            else
            {
                result.Char = chr;
            }

            //result = chr;

            matrix[y, x] = result;

            if (chr != 0x2800)
            {
                //console.log(`"${result}" "x: ${i}"\t"y: ${y}"\t"${JSON.stringify(item.color)}"`);
                cmatrix[y, x].Add(item.Color);
            }

            /*if (item.isInside({ x: i, y: y })) {
              matrix[y][i] = chr;
            } else {
              matrix[y][i] = String.fromCharCode(chCodeA | chCodeB);
            }*/

            x++;
        }
    }
}
