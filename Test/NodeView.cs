using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;
using Logires.Nodes;

namespace Test;

public class NodeView
{
    //private Node _node;
    //private IReadOnlyVector2<float> _position;

    public NodeView(Node node, IReadOnlyVector2<float> position, Color color)
    {
        Node = node ?? throw new ArgumentNullException(nameof(node));
        Position = position;//new RefVector2(position.X, position.Y);
        Color = color;
        Figure = new Ellipse(Position.Subtract(new Vector2(4, 2)), new Vector2(8, 4), true, Color);
    }

    public Node Node { get; private set; }
    public IReadOnlyVector2<float> Position { get; private set; }
    //public RefVector2 Position { get; private set; }
    public Color Color { get; private set; }
    public IFigure Figure { get; private set; }
}
