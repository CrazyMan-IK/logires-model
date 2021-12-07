using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class RangeGetter : Node, IHaveInputs, IHaveOutputs
{
    private readonly IntegerPin _inputA = new IntegerPin(true);
    private readonly IntegerPin _inputB = new IntegerPin(true);
    private readonly IteratorPin _output = new IteratorPin(false);

    public RangeGetter()
    {

    }

    public IEnumerable<IPin> Inputs
    {
        get
        {
            yield return _inputA;
            yield return _inputB;
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
        var min = Math.Min(_inputA.Value, _inputB.Value);
        var max = Math.Max(_inputA.Value, _inputB.Value);

        _output.Value = new LinkedList<object>(Enumerable.Range(min, max - min + 1).Select(x => (object)x)).First;
    }
}
