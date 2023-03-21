using System.Text;
using OneOf;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;
using BrailleCanvas.Models;

namespace BrailleCanvas.Figures;

public class Item : IFilledFigure
{
    //private readonly StringBuilder _builder = new StringBuilder();
    private int _oldHash = 0;
    //private string _internalText = "";

    public Item(string text, IReadOnlyVector2<float> position, bool isFilled, OneOf<Color, IHasValue<Color>> color)
    {
        Text = text;
        Size = Vector2.Zero;
        Position = position;
        Color = color;
        IsFilled = isFilled;
    }

    public string Text { get; set; }
    public IReadOnlyVector2<float> Size { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    public int? ZIndex { get; private set; }
    public OneOf<Color, IHasValue<Color>> Color { get; private set; }
    public bool IsFilled { get; private set; }

    public string StringValue()
    {    
        var newHash = Text.GetHashCode();

        if (_oldHash != newHash)
        {
            _oldHash = newHash;
            Update();
        }

        return Text;

        //return _internalText;

        /*var result = Text;

        result.Replace("\n", $"\n{new string(empty, (int)MathExtensions.Round(Position.X))}");

        return $"{new string('\n', (int)MathExtensions.Round(Position.Y))}{new string(empty, (int)MathExtensions.Round(Position.X))}{result}";*/
    }

    public bool IsInside(IReadOnlyVector2<float> point)
    {
        return point.X > Position.X && point.X < Position.X + Size.X &&
                     point.Y > Position.Y && point.Y < Position.Y + Size.Y;
    }

    private void Update()
    {
				//const char empty = (char)0x2800;
				
        var lines = Text.Split("\n");

        var x = lines.Select((line) => line.Length).Max();
        var y = lines.Length;

        Size = new Vector2(x, y);

        /*_builder.Clear();

        x = (int)MathExtensions.Round(Position.X);
        y = (int)MathExtensions.Round(Position.Y);

				_builder.Append('\n', y);
				_builder.Append(empty, x);

        var length = Text.Length;
        for	(int i = 0; i < length; i++)
        {
						var chr = Text[i];
        
        		if (chr == '\n')
        		{
        				_builder.Append(chr);
        				_builder.Append(empty, x);
        				continue;
        		}
        		
        		_builder.Append(chr);
        }

        //_internalText.Replace("\n", $"\n{new string(empty, (int)MathExtensions.Round(Position.X))}");

        //_internalText = $"{new string('\n', (int)MathExtensions.Round(Position.Y))}{new string(empty, (int)MathExtensions.Round(Position.X))}{_internalText}";

        _internalText = _builder.ToString();*/
    }
}
