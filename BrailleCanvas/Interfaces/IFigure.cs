using BrailleCanvas.Models;

namespace BrailleCanvas.Interfaces;

public interface IFigure
{
	IReadOnlyVector2<float> Size { get; }
	IReadOnlyVector2<float> Position { get; }
	int? ZIndex { get; }
	Color color { get; }

	string StringValue();
}

public interface IFilledFigure : IFigure
{
	bool IsFilled { get; }

	bool IsInside();
}
