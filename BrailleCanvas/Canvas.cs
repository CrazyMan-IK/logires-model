using System.Text;
using OneOf;
using BrailleCanvas.Models;
using BrailleCanvas.Extensions;
using BrailleCanvas.Interfaces;

namespace BrailleCanvas;

public class Canvas
{
    private readonly List<(int, IFigure)> _items = new List<(int, IFigure)>();
    private int _topZ = 0;
    private bool _canOverflow = false;
    private (List<OneOf<Color, Ternary<Color>>>, char)[,] _matrix;
    //private List<Color>[,] _cmatrix;
    private StringBuilder _result;

    public Canvas(IReadOnlyVector2<int> size, bool canOverflow = false)
    {
        Size = size;
        _canOverflow = canOverflow;

        _matrix = new (List<OneOf<Color, Ternary<Color>>>, char)[Size.Y, Size.X];
        //_cmatrix = new List<Color>[Size.Y, Size.X];

        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                _matrix[i, j] = (new List<OneOf<Color, Ternary<Color>>>(), '\u2800');
                //_cmatrix[i, j] = new List<Color>();
            }
        }

        _result = new StringBuilder((Size.X * Size.Y) * 2);
    }

    public IReadOnlyVector2<int> Size { get; private set; }

    public void Clear()
    {
        _items.Clear();
        _topZ = 0;
    }

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

    //public ReadOnlySpan<byte> StringValue()
    public string StringValue()
    {
        Sort();
        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                _matrix[i, j].Item1.Clear();
                _matrix[i, j].Item2 = '\u2800';
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
                Draw(item, _matrix, line, 0, y++);
            }
        }

        _result.Clear();
        for (int y = 0; y < _matrix.GetLength(0); y++)
        {
            for (int x = 0; x < _matrix.GetLength(1); x++)
            {
                if (_matrix[y, x].Item2 == '\u2800')
                {
                    _result.Append(_matrix[y, x].Item2);
                    continue;
                }

                
                var color = Color.MixColors(_matrix[y, x].Item1.Select(x => x.Match(x => x, x => x.Value)));
                //var color = colors[0];

                //if (colors.Count > 0)
                {
                    //console.log(getColorEscapeCharacter(color));
                    //matrix[y][x] = getColorEscapeCharacter(color) + matrix[y][x];
                    _result.Append(color.AsEscapeSequence());
                    _result.Append(_matrix[y, x].Item2);
                }
            }

            _result.Append('\n');
        }

        //var tst = from item in matrix
        //					select item;

        //Console.WriteLine(tst.GetType());
        //Console.WriteLine(matrix.Cast<string>().ToList().GetType());

        //return "";
        //return string.Join("\n", _matrix.GetRows().Select((row) => string.Join("", row)));

        return _result.ToString();

        /*var res = matrix.GetRows().SelectMany((row) => Encoding.UTF8.GetBytes(row.SelectMany(c => c?.StringValue() ?? "").Append('\n').ToArray()));

        var span = new ReadOnlySpan<byte>(res.ToArray());
        return span;*/
    }

    private void Sort()
    {
        _items.Sort((a, b) => a.Item1 - b.Item1);
    }

    private bool IsInBounds(int x)
    {
        return x < Size.X || _canOverflow;
    }

    private void Draw(IFigure item, (List<OneOf<Color, Ternary<Color>>>, char)[,] matrix, string str, int x, int y)
    {
        if (y < 0 || (y > Size.Y - 1 && !_canOverflow))
        {
            return;
        }

        /*if (x > 0)
        {
            for (int i = x; i > 0; i--)
            {
                if (matrix[y, i].Item2 == '\0' && IsInBounds(i))
                {
                    matrix[y, i].Item2 = '\u2800';
                }
            }
        }*/

        var fitem = item as IFilledFigure;
        var isFilled = false;
        if (fitem != null)
        {
            isFilled = fitem.IsFilled;
        }

        foreach (var chr in str)
        {
            if (!IsInBounds(x))
            {
                break;
            }

            //var chCodeA = (int)chr;//chr.charCodeAt(0);
            //var chCodeB = (matrix[y, x]?.GetCharCode() ?? 0x2800);//.charCodeAt(0);
            var oldChr = matrix[y, x].Item2;

            var result = '\0';
            if (chr >= 0x2800 && chr <= 0x28ff && oldChr >= 0x2800 && oldChr <= 0x28ff)
            {
                if (oldChr != 0x2800 && fitem != null && isFilled)
                {
                    var dots = Cell.GetDotsPositions(new Vector2Int(x, y));

                    result = Cell.Merge(chr - 0x2800, (chr | oldChr) - 0x2800, (a, b, index) =>
                    {
                        return fitem.IsInside(dots[7 - index]);
                    });

                    /*result.Merge((char)0xff, (a, b, index) =>
                    {
                        return fitem.IsInside(dots[7 - index]);
                    });*/
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
                    result = (char)(chr | oldChr);
                }
            }
            else
            {
                result = chr;
            }

            //result = chr;

            matrix[y, x].Item2 = result;

            if (chr != 0x2800)
            {
                //console.log(`"${result}" "x: ${i}"\t"y: ${y}"\t"${JSON.stringify(item.color)}"`);
                matrix[y, x].Item1.Add(item.Color);
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
