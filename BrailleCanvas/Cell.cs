using System;
using BrailleCanvas.Models;

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
	RB = 128
}

public class Cell
{
	public Cell()
	{
		
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
