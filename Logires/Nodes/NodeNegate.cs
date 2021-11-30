using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeNegate : Node, IHaveInputs, IHaveOutputs
{
	private BooleanPin _input = new BooleanPin(true);
	private BooleanPin _output = new BooleanPin(false);
	
	public NodeNegate()
	{
	  _output.Value = true;
	}
	
	public IEnumerable<Pin> Inputs => new[] { this._input };
	public IEnumerable<Pin> Outputs => new[] { this._output };
	
	public override void Update(long ticks)
	{
	  _output.Value = !_input.Value;
	}
}
