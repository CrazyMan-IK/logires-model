using Logires.Nodes;
using Logires.Interfaces;

namespace Logires;

public class Scheme : Node, IHaveInputs, IHaveOutputs
{
		private readonly List<Node> _nodes;

		public Scheme(IEnumerable<Node> nodes)
		{
				_nodes = nodes?.ToList() ?? new List<Node>();
		}

	  public IEnumerable<IPin> Inputs
	  {
	    get
	    {
	      yield break;
	    }
	  }
	  public IEnumerable<IPin> Outputs
	  {
	    get
	    {
	      yield break;
	    }
	  }

	  public void Add(Node node)
	  {
	  	  _nodes.Add(node);
	  }

	  public override void Update(long ticks)
	  {
	      foreach (var node in _nodes)
	      {
	      	  node.MarkDirty();
	      }
	      
	      foreach (var node in _nodes)
	      {
	      	  node.Tick(ticks);
	      }
	  }
}
