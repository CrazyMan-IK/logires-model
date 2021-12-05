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
	  _output.Value = !_input.Value;
	}
}
