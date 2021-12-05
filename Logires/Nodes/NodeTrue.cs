using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeTrue : Node, IHaveOutputs
{
	private BooleanPin _output = new BooleanPin(false);
	
	public NodeTrue()
	{
	  _output.Value = true;
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
