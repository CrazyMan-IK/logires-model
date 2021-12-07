using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class IteratorMoveNext : Node, IHaveInputs, IHaveOutputs
{
    private readonly IteratorPin _input = new IteratorPin(true);
    private readonly IteratorPin _output = new IteratorPin(false);

    public IteratorMoveNext()
    {

    }

    public IEnumerable<IPin> Inputs
    {
        get
        {
            yield return _input;
        }
    }
    public IEnumerable<IPin> Outputs
    {
        get
        {
            yield return _output;
        }
    }

    public override void Update(long ticks)
    {
        _output.Value = _input.Value?.Next;
    }
}
