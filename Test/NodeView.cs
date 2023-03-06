using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;
using Logires.Nodes;
using Test.Interfaces;

namespace Test;

public class NodeView : IHasVisual
{
    private readonly IFigure _figure;

    public NodeView(Node node, IReadOnlyVector2<float> position, Color color)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        Position = position;
        Color = color;
        _figure = new Ellipse(Position.Subtract(new Vector2(4, 2)), new Vector2(8, 4), true, Color);
    }

    public Node Node { get; }
    public IReadOnlyVector2<float> Position { get; }
    public Color Color { get; }
    public IEnumerable<IFigure> Visual
    {
    	  get
    	  {
    	  	  yield return _figure;
    	  }
    }
}
