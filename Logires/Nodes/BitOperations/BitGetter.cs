using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class BitGetter : Node, IHaveInputs, IHaveOutputs
{
    private readonly BitPin _inputA = new BitPin(true);
    private readonly IntegerPin _inputB = new IntegerPin(true);
    private readonly BooleanPin _output = new BooleanPin(false);

    public BitGetter()
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
        _output.Value = _inputA.Value.ElementAtOrDefault(_inputB.Value);
    }
}
