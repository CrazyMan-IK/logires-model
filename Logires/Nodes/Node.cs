using Logires.Interfaces;

namespace Logires.Nodes;

public abstract class Node : ITickable
{
	private bool _needUpdate = true;

	public void MarkDirty()
	{
		_needUpdate = true;
		
		if (this is IHaveOutputs haveOutputs)
		{
		  foreach (var output in haveOutputs.Outputs)
		  {
		    output.MarkDirty();
		  }
		}
	}

	public void Tick(long ticks)
	{
		//Console.WriteLine(GetType().Name);
	
		if (this is IHaveInputs haveInputs)
		{
		  foreach (var input in haveInputs.Inputs)
		  {
		    input.Update(ticks);
		  }

		  //Console.WriteLine("Have Inputs");
		  //Console.WriteLine(string.Join(", ", haveInputs.Inputs));
		}
		
		if (_needUpdate)
		{
		  _needUpdate = false;
		  Update(ticks);
		}
		
		if (this is IHaveOutputs haveOutputs)
		{
		  foreach (var output in haveOutputs.Outputs)
		  {
		    output.Update(ticks);
		  }

		  //Console.WriteLine("Have Outputs");
		  //Console.WriteLine(string.Join(", ", haveOutputs.Outputs));
		}

		//Console.WriteLine();
	}

	public abstract void Update(long ticks);
}
