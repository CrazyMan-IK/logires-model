using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeLog : Node, IHaveInputs
{
	public event Action<bool>? Logged = null;

	private readonly BooleanPin _input = new BooleanPin(true);
	
	public NodeLog()
	{
	  
	}
	
	public IEnumerable<IPin> Inputs
	{
	  get
	  {
	    yield return _input;
	  }
	}
	
	public override void Update(long ticks)
	{
	  Logged?.Invoke(_input.Value);
	}
}
