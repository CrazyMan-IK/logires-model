using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class Comparator : Node, IHaveInputs, IHaveOutputs
{
    private readonly BitPin _inputA = new BitPin(true);
    private readonly BitPin _inputB = new BitPin(true);
    private readonly BooleanPin _output = new BooleanPin(false);

    public Comparator()
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
        _output.Value = _inputA.Value.SequenceEqual(_inputB.Value);
    }
}
