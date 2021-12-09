using BrailleCanvas.Interfaces;

namespace Test.Interfaces;

public interface IHasVisual
{
    IEnumerable<IFigure> Visual { get; }
}
