using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeFalse : Node, IHaveOutputs
{
	private BooleanPin _output = new BooleanPin(false);
	
	public NodeFalse()
	{
	  
	}
	
	public IEnumerable<Pin> Outputs
	{
		get
		{
			yield return _output;
		}
	}
	
	public override void Update(long ticks)
	{
	  
	}
}
