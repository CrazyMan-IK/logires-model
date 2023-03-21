using Logires;
using Logires.Pins;
using Logires.Nodes;
using Logires.Interfaces;
using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;
using BrailleCanvas.Extensions;
using Test.Interfaces;

namespace Test;

public class SchemeView : ITickable, IHasVisual
{
    private readonly Scheme _scheme = new Scheme();
    private readonly List<NodeView> _nodes = new List<NodeView>();

    public SchemeView(IReadOnlyVector2<float> position, IReadOnlyVector2<float> size)
    {
        Position = position;
        Size = size;
    }

    public IReadOnlyVector2<float> Position { get; }
    public IReadOnlyVector2<float> Size { get; }
    public Node Scheme => _scheme;
    public IEnumerable<IFigure> Visual
    {
        get
        {
            var outCount = _scheme.InnerOutputs.Count();
            var inCount = _scheme.InnerInputs.Count();

            foreach (var node in _nodes)
            {
                if (node.Node is IHaveInputs haveInputs)
                {
                    foreach (var input in haveInputs.Inputs)
                    {
                        var i = 0;
                        foreach (var inOutput in _scheme.InnerOutputs)
                        {
                            if (!inOutput.IsConnectedWith(input))
                            {
                                continue;
                            }

                            yield return GetConnection(inOutput, node.Position, Position + new Vector2(0, Size.Y / (outCount + 1) * (i + 1)));

                            i++;
                        }
                    }
                }

                if (node.Node is not IHaveOutputs haveOutputs)
                {
                    continue;
                }

                foreach (var output in haveOutputs.Outputs)
                {
                    var i = 0;
                    foreach (var inInput in _scheme.InnerInputs)
                    {
                        if (!output.IsConnectedWith(inInput))
                        {
                            continue;
                        }

                        yield return GetConnection(output, node.Position, Position + new Vector2(Size.X, Size.Y / (inCount + 1) * (i + 1)));

                        i++;
                    }

                    foreach (var node2 in _nodes)
                    {
                        if (node == node2)
                        {
                            continue;
                        }

                        if (node2.Node is not IHaveInputs haveInputs2)
                        {
                            continue;
                        }

                        foreach (var input in haveInputs2.Inputs)
                        {
                            if (!output.IsConnectedWith(input))
                            {
                                continue;
                            }

                            yield return GetConnection(output, node.Position, node2.Position);
                        }
                    }
                }
            }

            foreach (var node in _nodes)
            {
                foreach (var visual in node.Visual)
                {
                    yield return visual;
                }
            }
        }
    }

    public void Add(NodeView view)
    {
        _scheme.Add(view.Node);
        _nodes.Add(view);
    }

    public IPin AddInput<T>(Pin<T> input)
    {
        return _scheme.AddInput<T>(input);
    }

    public IPin AddOutput<T>(Pin<T> output)
    {
        return _scheme.AddOutput<T>(output);
    }

    public void MarkDirty()
    {
        _scheme.MarkDirty();
    }

    public void Tick(long ticks)
    {
        _scheme.Tick(ticks);
    }

    private static IFigure GetConnection(IPin pin, params IReadOnlyVector2<float>[] positions)
    {
        if (pin is BooleanPin booleanPin)
        {
            return new Line(positions, new Ternary<Color>(() => booleanPin.Value, Constants.Green, Constants.Red));
        }
        else if (pin is BitPin bitPin)
        {
            return new Line(positions, new Gradient(() =>
            {
                var totalCount = bitPin.Value.Count;
                if (totalCount == 0)
                {
                    return 0;
                }

                var activeCount = bitPin.Value.Count(x => x);

                //return value * (1 - 0.333) + 0.333;
                //return (activeCount * 1.0f / totalCount) * (1 - 1 / 3.0f) + 1 / 3.0f;
                return 1 / 3.0f + (activeCount * 1.0f / totalCount) * 2 / 3.0f;
            }, Constants.Gray, Constants.Red, Constants.Blue, Constants.Green));
        }
        else
        {
            return new Line(positions, Constants.Yellow);
        }
    }
}
