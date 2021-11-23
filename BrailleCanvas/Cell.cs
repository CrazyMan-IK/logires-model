using System;
using BrailleCanvas.Models;
using BrailleCanvas.Interfaces;

namespace BrailleCanvas;

public enum CellState
{
	EMPTY = 0,
	LT = 1,
	LCT = 2,
	LCB = 4,
	RT = 8,
	RCT = 16,
	RCB = 32,
	LB = 64,
	RB = 128,
	UNKNOWN = 256
}

public class Cell
{
	private char _char = '\0';

	public Cell()
	{
		
	}

	public Cell(char chr)
	{
		Char = chr;
	}
	
	public Cell(CellState state)
	{
		if ((int)state > 255)
		{
			throw new ArgumentOutOfRangeException(nameof(state));
		}
		
		State = state;
	}

	public Cell(Vector2 position)
	{
		State = GetStateByPosition(position);
	}

	public CellState State { get; private set; } = CellState.EMPTY;
	public char Char
	{
		get => _char;
		set
		{
			_char = value;

			State = CellState.UNKNOWN;
			
			if (_char >= 0x2800 && _char <= 0x28FF)
			{
				State = (CellState)(_char - 0x2800);
			}
		}
	}

	public static Vector2[] GetDotsPositions(IReadOnlyVector2<int> pos)
	{
		return new Vector2[8]
		{
		  new Vector2(pos.X + 0.25f, pos.Y + 0.45f),
		  new Vector2(pos.X - 0.25f, pos.Y + 0.45f),
		  new Vector2(pos.X + 0.25f, pos.Y + 0.2f),
		  new Vector2(pos.X + 0.25f, pos.Y - 0.2f),
		  new Vector2(pos.X + 0.25f, pos.Y - 0.45f),
		  new Vector2(pos.X - 0.25f, pos.Y + 0.2f),
		  new Vector2(pos.X - 0.25f, pos.Y - 0.2f),
		  new Vector2(pos.X - 0.25f, pos.Y - 0.45f)
		};
	}

	public void Merge(Cell other, Func<bool, bool, int, bool> predicate)
	{
		if (other == null) {
		  throw new ArgumentNullException(nameof(other));
		}

		if (other.State == CellState.UNKNOWN)
		{
		  throw new InvalidOperationException();
		}

		Merge((char)other.State, predicate);
	}

	public void Merge(char other, Func<bool, bool, int, bool> predicate)
	{
		if (State == CellState.UNKNOWN)
		{
			throw new InvalidOperationException();
		}

		var a = (int)State;
		var b = (int)other;

		var c = 0;
		
		for (int i = 7; i >= 0; i--) {
		  var bitA = (a >> i) & 1; //a & (1 << i);
		  var bitB = (b >> i) & 1; //b & (1 << i);
		
		  c |= predicate(bitA != 0, bitB != 0, i) ? bitA : bitB;
		  //console.log(c.toString(2));
		  c <<= 1;
		}
		c >>= 1;
		//console.log(c);
		
		State = (CellState)c;
	}

	public int GetCharCode()
	{
		return (int)_char;
	}

	public string StringValue()
	{
		return "";
	}

	private CellState GetStateByPosition(Vector2 position)
	{
		var x = Math.Abs(position.X % 1);
		var y = Math.Abs(position.Y % 1);

		var res = CellState.EMPTY;
		if (x > 0.5)
		{
			if (y <= 0.25)
			{
				res = CellState.RT;
			}
			else if (y <= 0.50)
			{
				res = CellState.RCT;
			}
			else if (y <= 0.75)
			{
				res = CellState.RCB;
			}
			else
			{
				res = CellState.RB;
			}
		}
		else
		{
			if (y <= 0.25)
			{
				res = CellState.LT;
			}
			else if (y <= 0.50)
			{
				res = CellState.LCT;
			}
			else if (y <= 0.75)
			{
				res = CellState.LCB;
			}
			else
			{
				res = CellState.LB;
			}
		}

		return res;
	}
}
