using Logires;
using Logires.Pins;
using Logires.Nodes;
using Logires.Interfaces;
using BrailleCanvas;
using BrailleCanvas.Models;
using BrailleCanvas.Figures;
using BrailleCanvas.Interfaces;

namespace Test;

public class SchemeView : ITickable
{
		private readonly Scheme _scheme = new Scheme();
		private readonly List<NodeView> _nodes = new List<NodeView>();

		public Node Node => _scheme;

	  public IEnumerable<IFigure> Visual
	  {
	  	  get
	  	  {
	  	  	  foreach (var node in _nodes)
	  	  	  {
                if (node.Node is IHaveInputs haveInputs)
                {
                	  foreach (var input in haveInputs.Inputs)
                	  {
                	  	  foreach (var inOutput in _scheme.InnerOutputs)
                	  	  {
                	  	  	  if (!inOutput.IsConnectedWith(input))
                	  	  	  {
                	  	  	  	  continue;
                	  	  	  }

                	  	  	  yield return GetConnection(inOutput, new[] { node.Position, new Vector2(0, 12.5f) });
                	  	  }
                	  }
                }
                
	  	  	      if (node.Node is not IHaveOutputs haveOutputs)
	  	  	      {
	  	  	          continue;
	  	  	      }
	  	  	  
	  	  	      foreach (var output in haveOutputs.Outputs)
	  	  	      {
	  	  	          /*if (output is not BooleanPin booleanOutput)
	  	  	          {
	  	  	              continue;
	  	  	          }*/

	  	  	          foreach (var inInput in _scheme.InnerInputs)
	  	  	          {
	  	  	          	  if (!output.IsConnectedWith(inInput))
	  	  	          	  {
	  	  	          	  	  continue;
	  	  	          	  }

                	  	  yield return GetConnection(output, new[] { node.Position, new Vector2(76, 12.5f) });
	  	  	          }
	  	  	  
	  	  	          foreach (var node2 in _nodes)
	  	  	          {
	  	  	              if (node == node2)
	  	  	              {
	  	  	                  continue;
	  	  	              }
	  	  	  
	  	  	              if (node2.Node is not IHaveInputs haveInputs2)
	  	  	              {
	  	  	                  continue;
	  	  	              }
	  	  	  
	  	  	              foreach (var input in haveInputs2.Inputs)
	  	  	              {
	  	  	                  if (!output.IsConnectedWith(input))
	  	  	                  {
	  	  	                      continue;
	  	  	                  }

	  	  	                  yield return GetConnection(output, new[] { node.Position, node2.Position });
	  	  	  
	  	  	                  /*if (output is BooleanPin booleanOutput)
	  	  	                  {
	  	  	                      yield return new Line(new[] { node.Position, node2.Position }, new Ternary<Color>(() => booleanOutput.Value, Constants.Green, Constants.Red));
	  	  	                  }
	  	  	                  else if (output is BitPin bitOutput)
	  	  	                  {
	  	  	                      yield return new Line(new[] { node.Position, node2.Position }, new Gradient(() =>
	  	  	                      {
	  	  	                          var totalCount = bitOutput.Value.Count;
	  	  	                          if (totalCount == 0)
	  	  	                          {
	  	  	                              return 0;
	  	  	                          }
	  	  	  
	  	  	                          var activeCount = bitOutput.Value.Count(x => x);
	  	  	  
	  	  	                          return activeCount * 1.0f / totalCount;
	  	  	                      }, Constants.Red, Constants.Blue, Constants.Green));
	  	  	                  }
	  	  	                  else
	  	  	                  {
	  	  	                      yield return new Line(new[] { node.Position, node2.Position }, Constants.Yellow);
	  	  	                  }*/
	  	  	              }
	  	  	          }
	  	  	      }
	  	  	  }
 	  	  	  
	  	  	  foreach (var node in _nodes)
 	  	  	  {
 	  	  	  		foreach (var visual in node.Visual)
 	  	  	  		{
 	  	  	  		    yield return visual;
 	  	  	  		}
 	  	  	  }
	  	  }
	  }

	  public void Add(NodeView view)
	  {
	  		_scheme.Add(view.Node);
	  	  _nodes.Add(view);
	  }

	  public IPin AddInput<T>(Pin<T> input)
	  {
	  	  return _scheme.AddInput<T>(input);
	  }
	  
	  public IPin AddOutput<T>(Pin<T> output)
	  {
	  	  return _scheme.AddOutput<T>(output);
	  }

	  public void MarkDirty()
	  {
	  	  _scheme.MarkDirty();
	  }

	  public void Tick(long ticks)
	  {
	  	  _scheme.Tick(ticks);
	  }

	  private IFigure GetConnection(IPin pin, IEnumerable<IReadOnlyVector2<float>> positions)
	  {
	  	  if (pin is BooleanPin booleanPin)
	  	  {
	  	      return new Line(positions, new Ternary<Color>(() => booleanPin.Value, Constants.Green, Constants.Red));
	  	  }
	  	  else if (pin is BitPin bitPin)
        {
            return new Line(positions, new Gradient(() =>
            {
                var totalCount = bitPin.Value.Count;
                if (totalCount == 0)
                {
                    return 0;
                }

                var activeCount = bitPin.Value.Count(x => x);

                return activeCount * 1.0f / totalCount;
            }, Constants.Red, Constants.Blue, Constants.Green));
        }
        else
        {
            return new Line(positions, Constants.Yellow);
        }
	  }
}
