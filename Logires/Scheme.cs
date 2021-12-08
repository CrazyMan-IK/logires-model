using Logires.Nodes;
using Logires.Interfaces;
using Logires.Pins;

namespace Logires;

public class Scheme : Node, IHaveInputs, IHaveOutputs
{
    private readonly List<Node> _nodes;
    private readonly List<IPin> _outerInputs = new List<IPin>();
    private readonly List<IPin> _innerOutputs = new List<IPin>();
    private readonly List<IPin> _outerOutputs = new List<IPin>();
    private readonly List<IPin> _innerInputs = new List<IPin>();

    public Scheme(IEnumerable<Node> nodes)
    {
        _nodes = nodes?.ToList() ?? new List<Node>();
    }

    public IEnumerable<IPin> Inputs => _outerInputs;
    public IEnumerable<IPin> Outputs => _outerOutputs;

    public void Add(Node node)
    {
        _nodes.Add(node);
    }

    public IPin AddInput<T>(Pin<T> input)
    {
        var outer = input.Clone(true);
        var inner = input.Clone(false);

        _outerInputs.Add(outer);
        _innerOutputs.Add(inner);

        //_inputLinks.Add(outer);

				outer.ConnectReceiver(inner);
        input.Connect(inner);

        return outer;
    }

    public IPin AddOutput<T>(Pin<T> output)
    {
        var outer = output.Clone(false);
        var inner = output.Clone(true);

        _outerOutputs.Add(outer);
        _innerInputs.Add(inner);

        //_outputLinks.Add(inner);

				inner.ConnectReceiver(outer);
        output.Connect(inner);

        return outer;
    }

    public override void Update(long ticks)
    {
        /*foreach (var link in _inputLinks)
        {
            //link.Item2.SetValueFrom(link.Item1);
            link.Item1.RequestUpdate(ticks);
        }*/

        foreach (var node in _nodes)
        {
            node.MarkDirty();
        }

        foreach (var node in _nodes)
        {
            node.Tick(ticks);
        }

        foreach (var inner in _innerInputs)
        {
            //link.Item2.SetValueFrom(link.Item1);
            inner.RequestUpdate(ticks);
        }
        
        Console.WriteLine(_innerInputs[0]);
        Console.WriteLine(_outerOutputs[0]);
    }
}
