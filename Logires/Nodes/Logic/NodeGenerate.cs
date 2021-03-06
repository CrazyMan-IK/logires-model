using Logires.Pins;

using Logires.Interfaces;

namespace Logires.Nodes;

public class NodeGenerate : Node, IHaveOutputs
{
	private readonly BooleanPin _output = new BooleanPin(false);
	private readonly int _frequency = 1;
	
	public NodeGenerate(int freq = 1)
	{
	  _frequency = freq;
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
	  /*if (ticks % _frequency != 0)
	  {
	    return;
	  }
	  
	  _output.Value = !_output.Value;*/

	  _output.Value = Math.Floor(ticks * 1.0 / _frequency) % 2 != 0;
	}
}
