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

    public Scheme() : this(new List<Node>())
    {
          
    }

    public Scheme(IEnumerable<Node> nodes)
    {
        _nodes = nodes.ToList();

        if (HasRecursion(new List<Scheme>()))
        {
              throw new InvalidOperationException();
        }
    }

    public IEnumerable<IPin> Inputs => _outerInputs;
    public IEnumerable<IPin> InnerOutputs => _innerOutputs;
    public IEnumerable<IPin> Outputs => _outerOutputs;
    public IEnumerable<IPin> InnerInputs => _innerInputs;
    public IEnumerable<Node> Nodes => _nodes;

    public void Add(Node node)
    {
        _nodes.Add(node);

        if (HasRecursion(new List<Scheme>()))
        {
              throw new InvalidOperationException();
        }
    }

    public bool HasRecursion(List<Scheme> schemes)
    {
          if (_nodes.Intersect(schemes).Any())
          {
              return true;
          }

          schemes.Add(this);

          foreach (var node in _nodes)
          {
              if (node is Scheme other)
              {
                  if (other == this || other.HasRecursion(schemes))
                  {
                      return true;
                  }
              }
          }

          return false;
    }

    public IPin AddInput<T>(Pin<T> input)
    {
        var outer = (Pin<T>)Activator.CreateInstance(input.GetType(), (object)true)!;
        var inner = (Pin<T>)Activator.CreateInstance(input.GetType(), (object)false)!;

        _outerInputs.Add(outer);
        _innerOutputs.Add(inner);

        outer.ConnectReceiver(inner);
        input.Connect(inner);

        return outer;
    }

    public IPin AddOutput<T>(Pin<T> output)
    {
        var outer = (Pin<T>)Activator.CreateInstance(output.GetType(), (object)false)!;
        var inner = (Pin<T>)Activator.CreateInstance(output.GetType(), (object)true)!;

        _outerOutputs.Add(outer);
        _innerInputs.Add(inner);

        inner.ConnectReceiver(outer);
        output.Connect(inner);

        return outer;
    }

    public override void Update(long ticks)
    {
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
            inner.Update(ticks);
        }
    }
}
