using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeAnd : Node, IHaveInputs, IHaveOutputs
{
	private BooleanPin _inputA = new BooleanPin(true);
	private BooleanPin _inputB = new BooleanPin(true);
	private BooleanPin _output = new BooleanPin(false);
	
	public NodeAnd()
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
	  _output.Value = _inputA.Value && _inputB.Value;
	}
}
