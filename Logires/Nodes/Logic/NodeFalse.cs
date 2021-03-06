using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeFalse : Node, IHaveOutputs
{
	private readonly BooleanPin _output = new BooleanPin(false);
	
	public NodeFalse()
	{
	  
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
	  
	}
}
